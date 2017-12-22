using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Format;

namespace Jhu.SkyQuery.Format.VOTable
{
    /// <summary>
    /// Implements functionality responsible for reading and writing a single
    /// resource block within a VOTable.
    /// </summary>
    [Serializable]
    public class VOTableResource : XmlDataFileBlock, ICloneable
    {
        private delegate object BinaryColumnReader(Column column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter);
        private delegate void BinaryColumnWriter(Column column, byte[] buffer, SharpFitsIO.BitConverterBase bitConverter, object value);

        private VOTableSerialization serialization;
        private SharpFitsIO.BitConverterBase bitConverter;
        private byte[] binaryBuffer;
        private BinaryColumnReader[] columnReaders;
        private BinaryColumnWriter[] columnWriters;

        /// <summary>
        /// Gets the objects wrapping the whole VOTABLE file.
        /// </summary>
        private VOTable File
        {
            get { return (VOTable)file; }
        }

        #region Constructors and initializers

        /// <summary>
        /// Initializes a VOTable resource block object.
        /// </summary>
        /// <param name="file"></param>
        public VOTableResource(VOTable file)
            : base(file)
        {
            InitializeMembers();
        }

        public VOTableResource(VOTableResource old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.serialization = VOTableSerialization.Unknown;
            this.bitConverter = null;
            this.binaryBuffer = null;
        }

        private void CopyMembers(VOTableResource old)
        {
            this.serialization = old.serialization;
            this.bitConverter = null;
            this.binaryBuffer = null;
        }

        public override object Clone()
        {
            return new VOTableResource(this);
        }

        #endregion
        #region Framework reader functions

        /// <summary>
        /// Reads the header of VOTable resource, from the TABLE tag to the TABLEDATA tag.
        /// </summary>
        protected override Task OnReadHeaderAsync()
        {
            ReadResourceElement();

            return Task.CompletedTask;
        }

        protected override void OnSetMetadata(int blockCounter)
        {
            base.OnSetMetadata(blockCounter);

            // TODO: do nothing here, this will be refactored
        }

        protected override Task<bool> OnReadNextRowAsync(object[] values)
        {
            // If the RESOURCE contains a DATATABLE then the string contents
            // of the TD tags can be returned and the XmlDataFileBlock class will
            // take care of the data conversion, so simply call

            switch (serialization)
            {
                case VOTableSerialization.TableData:
                    return base.OnReadNextRowAsync(values);
                case VOTableSerialization.Binary:
                case VOTableSerialization.Binary2:
                    return ReadNextRowFromStreamAsync(values);
                case VOTableSerialization.Fits:
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Completes reading of a resource by reading its closing tag.
        /// </summary>
        protected override async Task OnReadFooterAsync()
        {
            // Make sure the ending RESOURCE tag is read and the reader
            // is positioned at the next tag

            // The TABLE element and the RESOURCE element can contain
            // trailing INFO tags (what are these for?)
            // make sure that they are read and position the reader after the
            // closing RESOURCE element, whatever it is.

            while (File.XmlReader.NodeType != XmlNodeType.EndElement ||
                   VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagData) != 0)
            {
                switch (File.Version)
                {
                    case VOTableVersion.V1_1:
                        File.Deserialize<V1_1.Info>();
                        break;
                    case VOTableVersion.V1_2:
                        File.Deserialize<V1_2.Info>();
                        break;
                    case VOTableVersion.V1_3:
                        File.Deserialize<V1_3.Info>();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            File.XmlReader.ReadEndElement();

            while (File.XmlReader.NodeType != XmlNodeType.EndElement ||
                   VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTable) != 0)
            {
                switch (File.Version)
                {
                    case VOTableVersion.V1_1:
                        File.Deserialize<V1_1.Info>();
                        break;
                    case VOTableVersion.V1_2:
                        File.Deserialize<V1_2.Info>();
                        break;
                    case VOTableVersion.V1_3:
                        File.Deserialize<V1_3.Info>();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            File.XmlReader.ReadEndElement();

            while (File.XmlReader.NodeType != XmlNodeType.EndElement ||
                   VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagResource) != 0)
            {
                switch (File.Version)
                {
                    case VOTableVersion.V1_1:
                        File.Deserialize<V1_1.Info>();
                        break;
                    case VOTableVersion.V1_2:
                        File.Deserialize<V1_2.Info>();
                        break;
                    case VOTableVersion.V1_3:
                        File.Deserialize<V1_3.Info>();
                        break;
                    default:
                        throw new NotImplementedException();
                }

                // TODO: in reality, the RESOURCE tag could contain additional special tags
            }

            File.XmlReader.ReadEndElement();
        }

        /// <summary>
        /// Completes reading of a table and stops on the last tag.
        /// </summary>
        /// <remarks>
        /// This function is called by the infrastructure to read all possible data
        /// rows that the client didn't consume.
        /// </remarks>
        protected override Task OnReadToFinishAsync()
        {
            // This can be called by the framework anywhere within a RESOURCE tag

            // Make sure that binary mode components are destroyed
            binaryBuffer = null;
            bitConverter = null;

            // Make sure that the reader is position right after the
            // closing TABLEDATA/BINARY/BINARY2/FITS tag. There might be INFO tags
            // that will be read in the footer
            while (File.XmlReader.NodeType != XmlNodeType.EndElement ||
                   File.XmlReader.NodeType == XmlNodeType.EndElement &&
                   VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTableData) != 0 &&
                   VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagBinary) != 0 &&
                   VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagBinary2) != 0 &&
                   VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagFits) != 0)
            {
                File.XmlReader.Read();
            }

            File.XmlReader.ReadEndElement();

            // Now the reader is positioned after the TABLEDATA/BINARY/BINARY2/FITS tags
            // Footer reader will consume the rest of the tags.

            return Task.CompletedTask;
        }

        /// <summary>
        /// Returns an array of strings containing data from the next data row.
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="skipComments"></param>
        /// <returns></returns>
        protected override Task<bool> OnReadNextRowPartsAsync(List<string> parts, bool skipComments)
        {
            parts.Clear();

            if (File.XmlReader.NodeType == XmlNodeType.EndElement &&
                (VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTableData) == 0 ||
                 VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagData) == 0))
            {
                // End of table
                return Task.FromResult(false);
            }
            else
            {
                // Consume TR tag
                File.XmlReader.ReadStartElement(Constants.TagTR);

                var q = 0;
                // Read the TD tags
                while (true)
                {
                    if (File.XmlReader.NodeType == XmlNodeType.Element &&
                        VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTD) == 0)
                    {

                        if (!File.XmlReader.IsEmptyElement)
                        {
                            File.XmlReader.ReadStartElement(Constants.TagTD);
                            // A cell found
                            parts.Add(File.XmlReader.ReadString());

                            // Consume closing tag
                            File.XmlReader.ReadEndElement();
                        }
                        else
                        {
                            parts.Add(null);
                            File.XmlReader.Read();
                        }
                    }
                    else if (File.XmlReader.NodeType == XmlNodeType.EndElement &&
                        VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTR) == 0)
                    {
                        // End of a row found
                        File.XmlReader.ReadEndElement();

                        break;
                    }
                    else
                    {
                        throw new FileFormatException();    // *** TODO
                    }
                    q++;
                }

                return Task.FromResult(true);
            }
        }

        #endregion
        #region VOTable reader functions

        private void ReadResourceElement()
        {
            // The reader is now positioned on the RESOURCE tag   
            File.XmlReader.ReadStartElement(Constants.TagResource);

            // Read until the the TABLE tag is found and call the specific reader function
            // which will then read the table header and stop at the DATA tag.
            // If no table tag found we will bump into the closing </RESOURCE> tag anyway,
            // so throw and exception there
            // Also, throw exception on embeded RESOURCE tags.

            while (File.XmlReader.NodeType == XmlNodeType.Element &&
                VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTable) != 0)
            {
                // Stop criterium: </RESOURCE>
                if (File.XmlReader.NodeType == XmlNodeType.EndElement &&
                    VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagResource) == 0)
                {
                    throw Error.TableNotFound();
                }

                switch (File.Version)
                {
                    case VOTableVersion.V1_1:
                        switch (File.XmlReader.Name)
                        {
                            case Constants.TagDescription:
                                File.Deserialize<V1_1.Description>();
                                break;
                            case Constants.TagInfo:
                                File.Deserialize<V1_1.Info>();
                                break;
                            case Constants.TagCoosys:
                                File.Deserialize<V1_1.Coosys>();
                                break;
                            case Constants.TagParam:
                                File.Deserialize<V1_1.Param>();
                                break;
                            case Constants.TagLink:
                                File.Deserialize<V1_1.Link>();
                                break;
                            case Constants.TagResource:
                                throw Error.RecursiveResourceNotSupported();
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    case VOTableVersion.V1_2:
                        switch (File.XmlReader.Name)
                        {
                            case Constants.TagDescription:
                                File.Deserialize<V1_2.Description>();
                                break;
                            case Constants.TagInfo:
                                File.Deserialize<V1_2.Info>();
                                break;
                            case Constants.TagCoosys:
                                File.Deserialize<V1_2.Coosys>();
                                break;
                            case Constants.TagGroup:
                                File.Deserialize<V1_2.Group>();
                                break;
                            case Constants.TagParam:
                                File.Deserialize<V1_2.Param>();
                                break;
                            case Constants.TagLink:
                                File.Deserialize<V1_2.Link>();
                                break;
                            case Constants.TagResource:
                                throw Error.RecursiveResourceNotSupported();
                            default:
                                throw new NotImplementedException();
                        }
                        break;

                    case VOTableVersion.V1_3:
                        switch (File.XmlReader.Name)
                        {
                            case Constants.TagDescription:
                                File.Deserialize<V1_3.Description>();
                                break;
                            case Constants.TagInfo:
                                File.Deserialize<V1_3.Info>();
                                break;
                            case Constants.TagCoosys:
                                File.Deserialize<V1_3.Coosys>();
                                break;
                            case Constants.TagGroup:
                                File.Deserialize<V1_3.Group>();
                                break;
                            case Constants.TagParam:
                                File.Deserialize<V1_3.Param>();
                                break;
                            case Constants.TagLink:
                                File.Deserialize<V1_3.Link>();
                                break;
                            case Constants.TagResource:
                                throw Error.RecursiveResourceNotSupported();
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }

                File.XmlReader.MoveToContent();
            }

            ReadTableElement();
            ReadDataElement();
            // Reader is positioned on the first TR tag or the STREAM tag now
        }

        private void ReadTableElement()
        {
            // While processing the above tags, collect info on columns
            var columns = new List<Column>();
            int fieldcount = 0;

            File.XmlReader.ReadStartElement(Constants.TagTable);

            while (File.XmlReader.NodeType == XmlNodeType.Element &&
                   XmlDataFile.Comparer.Compare(File.XmlReader.Name, Constants.TagData) != 0)
            {
                // Stop criterium: </TABLE>
                if (File.XmlReader.NodeType == XmlNodeType.EndElement &&
                    VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTable) == 0)
                {
                    throw Error.DataNotFound();
                }

                V1_1.Field field = null;

                switch (File.Version)
                {
                    case VOTableVersion.V1_1:
                        switch (File.XmlReader.Name)
                        {
                            case Constants.TagDescription:
                                File.Deserialize<V1_1.Description>();
                                break;
                            case Constants.TagField:
                                field = File.Deserialize<V1_1.Field>();
                                break;
                            case Constants.TagParam:
                                File.Deserialize<V1_1.Param>();
                                break;
                            case Constants.TagGroup:
                                File.Deserialize<V1_1.Group>();
                                break;
                            case Constants.TagLink:
                                File.Deserialize<V1_1.Link>();
                                throw Error.LinksNotSupported();
                            default:
                                throw new NotImplementedException();
                        }
                        break;

                    case VOTableVersion.V1_2:
                        switch (File.XmlReader.Name)
                        {
                            case Constants.TagDescription:
                                File.Deserialize<V1_2.Description>();
                                break;
                            case Constants.TagInfo:
                                File.Deserialize<V1_2.Info>();
                                break;
                            case Constants.TagField:
                                field = File.Deserialize<V1_2.Field>();
                                break;
                            case Constants.TagParam:
                                File.Deserialize<V1_2.Param>();
                                break;
                            case Constants.TagGroup:
                                File.Deserialize<V1_2.Group>();
                                break;
                            case Constants.TagLink:
                                File.Deserialize<V1_2.Link>();
                                throw Error.LinksNotSupported();
                            default:
                                throw new NotImplementedException();
                        }
                        break;

                    case VOTableVersion.V1_3:
                        switch (File.XmlReader.Name)
                        {
                            case Constants.TagDescription:
                                File.Deserialize<V1_3.Description>();
                                break;
                            case Constants.TagInfo:
                                File.Deserialize<V1_3.Info>();
                                break;
                            case Constants.TagField:
                                field = File.Deserialize<V1_3.Field>();
                                break;
                            case Constants.TagParam:
                                File.Deserialize<V1_3.Param>();
                                break;
                            case Constants.TagGroup:
                                File.Deserialize<V1_3.Group>();
                                break;
                            case Constants.TagLink:
                                File.Deserialize<V1_3.Link>();
                                throw Error.LinksNotSupported();
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                }

                if (field != null)
                {
                    var c = field.CreateColumn();
                    c.ID = fieldcount;
                    fieldcount++;
                    columns.Add(c);
                }

                File.XmlReader.MoveToContent();
            }

            // At this point we have all info on columns, so
            // call CreateColumns on base class

            CreateColumns(columns);

            // The reader is now positioned on a DATA tag
        }

        private void ReadDataElement()
        {
            File.XmlReader.ReadStartElement(Constants.TagData);
            File.XmlReader.MoveToContent();

            if (VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTableData) == 0)
            {
                serialization = VOTableSerialization.TableData;
                ReadTableDataElement();
            }
            else if (VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagBinary) == 0)
            {
                serialization = VOTableSerialization.Binary;
                ReadBinaryElement();
            }
            else if (VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagBinary2) == 0)
            {
                serialization = VOTableSerialization.Binary2;
                ReadBinary2Element();
            }
            else if (VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagFits) == 0)
            {
                throw Error.UnsupportedSerialization(VOTableSerialization.Fits);
            }
        }

        private void ReadTableDataElement()
        {
            // TODO: position reader on the very first TR tag
            // subsequent processing will be done when OnReadNextRow is called
            // by the framework

            File.XmlReader.ReadStartElement(Constants.TagTableData);

            // All subsequent tags will be read row-by-row
        }

        #endregion
        #region Binary serialization support

        private void ReadBinaryElement()
        {
            File.XmlReader.ReadStartElement(Constants.TagBinary);
            File.XmlReader.MoveToContent();
            ReadStreamElement();
        }

        private void ReadBinary2Element()
        {
            File.XmlReader.ReadStartElement(Constants.TagBinary2);
            File.XmlReader.MoveToContent();
            ReadStreamElement();
        }

        private void ReadStreamElement()
        {
            // The reader is now positioned on a STREAM element
            if (File.XmlReader.GetAttribute(Constants.AttributeHref) != null)
            {
                throw Error.ReferencedStreamsNotSupported();
            }

            // Figure out content encoding
            var encattr = File.XmlReader.GetAttribute(Constants.AttributeEncoding);

            if (encattr == null)
            {
                throw Error.EncodingNotFound();
            }

            if (!Enum.TryParse(encattr, true, out VOTableEncoding encoding) ||
                encoding != VOTableEncoding.Base64)
            {
                throw Error.EncodingNotSupported(encattr);
            }

            File.XmlReader.ReadStartElement(Constants.TagStream);
            // Now the reader is positioned on a base64 encoded binary stream

            CreateColumnReaders();
            bitConverter = new SharpFitsIO.SwapBitConverter();

            // TODO: estimate size of stride buffer
            binaryBuffer = new byte[0x10000];
        }

        private async Task<bool> ReadNextRowFromStreamAsync(object[] values)
        {
            // TODO: implement arrays

            try
            {
                if (serialization == VOTableSerialization.Binary2)
                {
                    int prefixlen = (Columns.Count + 7) / 8;

                    var s = await File.XmlReader.ReadContentAsBase64Async(binaryBuffer, 0, prefixlen);

                    if (prefixlen != s)
                    {
                        return false;
                    }

                    for (int i = 0; i < Columns.Count; i++)
                    {
                        int bb = i / 8;
                        int bi = i - bb * 8;

                        if (((binaryBuffer[bb] << bi) & 0x80) != 0)
                        {
                            values[i] = DBNull.Value;
                        }
                        else
                        {
                            values[i] = null;
                        }
                    }
                }

                for (int i = 0; i < Columns.Count; i++)
                {
                    if (values[i] != DBNull.Value)
                    {
                        var column = Columns[i];

                        if (column.DataType.IsFixedLength)
                        {
                            var l = column.DataType.ByteSize;

                            if (column.DataType.HasLength)
                            {
                                l *= column.DataType.Length;
                            }

                            var s = await File.XmlReader.ReadContentAsBase64Async(binaryBuffer, 0, l);

                            if (l != s)
                            {
                                return false;
                            }

                            values[i] = columnReaders[i](column, binaryBuffer, l, bitConverter);
                        }
                        else
                        {
                            var l = 4;
                            var s = await File.XmlReader.ReadContentAsBase64Async(binaryBuffer, 0, l);

                            if (l != s)
                            {
                                return false;
                            }

                            var length = bitConverter.ToInt32(binaryBuffer, 0);
                            l = column.DataType.ByteSize * length;

                            // If stride buffer is not enough, increase
                            if (l > binaryBuffer.Length)
                            {
                                binaryBuffer = new byte[l]; // TODO: round up to 64k blocks?
                            }

                            s = await File.XmlReader.ReadContentAsBase64Async(binaryBuffer, 0, l);

                            if (l != s)
                            {
                                return false;
                            }

                            values[i] = columnReaders[i](column, binaryBuffer, length, bitConverter);
                        }
                    }
                }

                return true;
            }
            catch (EndOfStreamException)
            {
                bitConverter = null;
                binaryBuffer = null;

                return false;
            }
        }

        private void CreateColumnReaders()
        {
            columnReaders = new BinaryColumnReader[Columns.Count];

            for (int i = 0; i < columnReaders.Length; i++)
            {
                var datatype = Columns[i].DataType;
                var type = datatype.Type;

                // TODO: how to deal with bit arrays?
                // TODO: how to deal with arrays in general?

                if (type == typeof(Boolean))
                {
                    columnReaders[i] = delegate (Column column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                    {
                        return buffer[0] != 0;
                    };
                }
                else if (type == typeof(Byte))
                {
                    columnReaders[i] = delegate (Column column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                    {
                        return buffer[0];
                    };
                }
                else if (type == typeof(Int16))
                {
                    columnReaders[i] = delegate (Column column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                    {
                        return bitConverter.ToInt16(buffer, 0);
                    };
                }
                else if (type == typeof(Int32))
                {
                    columnReaders[i] = delegate (Column column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                    {
                        return bitConverter.ToInt32(buffer, 0);
                    };
                }
                else if (type == typeof(Int64))
                {
                    columnReaders[i] = delegate (Column column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                    {
                        return bitConverter.ToInt64(buffer, 0);
                    };
                }
                else if (type == typeof(Single))
                {
                    columnReaders[i] = delegate (Column column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                    {
                        return bitConverter.ToSingle(buffer, 0);
                    };
                }
                else if (type == typeof(Double))
                {
                    columnReaders[i] = delegate (Column column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                    {
                        return bitConverter.ToDouble(buffer, 0);
                    };
                }
                else if (type == typeof(SingleComplex))
                {
                    columnReaders[i] = delegate (Column column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                    {
                        SingleComplex c;
                        c.A = bitConverter.ToSingle(buffer, 0);
                        c.B = bitConverter.ToSingle(buffer, 4);
                        return c;
                    };
                }
                else if (type == typeof(DoubleComplex))
                {
                    columnReaders[i] = delegate (Column column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                    {
                        DoubleComplex c;
                        c.A = bitConverter.ToDouble(buffer, 0);
                        c.B = bitConverter.ToDouble(buffer, 8);
                        return c;
                    };
                }
                else if (type == typeof(string))
                {
                    if (datatype.IsUnicode)
                    {
                        // Unicode
                        columnReaders[i] = delegate (Column column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                        {
                            return Encoding.Unicode.GetChars(buffer, 0, 2 * length);
                        };
                    }
                    else
                    {
                        // ASCII
                        // Fixed length
                        columnReaders[i] = delegate (Column column, byte[] buffer, int length, SharpFitsIO.BitConverterBase bitConverter)
                        {
                            return Encoding.ASCII.GetChars(buffer, 0, length);
                        };
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        #endregion

        // THIS IS OLD CODE, REUSE OR DELETE
#if false

        #region Column functions

        /// <summary>
        /// Reads the resource header field tags to extract columns. No tags after
        /// the data tag is processed, so all columns must be listed before the actual data.
        /// </summary>
        private void DetectColumns()
        {
            // Read a series for FIELD tags
            var cols = new List<Column>();

            while (true)
            {
                if (File.XmlReader.NodeType == XmlNodeType.Element &&
                    VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagField) == 0)
                {
                    // Column found

                    var col = new Column()
                    {
                        Name = File.XmlReader.GetAttribute(Constants.AttributeName),
                        DataType = GetVOTableDataType(),
                        Metadata = GetVOTableMetaData(),
                    };

                    cols.Add(col);

                    File.XmlReader.Read();
                }
                else if (VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagData) == 0)
                {
                    // A DATA tag is detected
                    // End of header reached, return
                    break;
                }
                else
                {
                    // Skip GROUP, PARAM, DESCRIPTION, etc.
                    File.XmlReader.Read();
                }
            }

            CreateColumns(cols.ToArray());
        }

        /// <summary>
        /// Figures out the data type from a field tag.
        /// </summary>
        /// <param name="voTableType"></param>
        /// <param name="arraySizeString"></param>
        /// <returns></returns>
        private DataType GetVOTableDataType()
        {
            var datatype = File.XmlReader.GetAttribute(Constants.AttributeDataType);
            var arraysize = File.XmlReader.GetAttribute(Constants.AttributeArraySize);

            int arraySize;
            bool arrayVariable;
            GetArrayDimensions(arraysize, out arraySize, out arrayVariable);

            // TODO: implement arrays
            // TODO: use constants for data type names
            DataType dt;

            switch (datatype.ToLowerInvariant())
            {
                case Constants.TypeBoolean:
                    dt = DataTypes.Boolean;
                    break;
                case Constants.TypeBit:
                    dt = DataTypes.Boolean;    // This is union bit
                    break;
                case Constants.TypeByte:
                    if (arraySize == -1)
                    {
                        dt = DataTypes.SqlVarBinaryMax;
                    }
                    else if (arrayVariable)
                    {
                        dt = DataTypes.SqlVarBinary;
                        dt.Length = arraySize;
                    }
                    else
                    {
                        dt = DataTypes.SqlBinary;
                        dt.Length = arraySize;
                    }
                    break;
                case Constants.TypeShort:
                    dt = DataTypes.SqlSmallInt;
                    break;
                case Constants.TypeInt:
                    dt = DataTypes.SqlInt;
                    break;
                case Constants.TypeLong:
                    dt = DataTypes.SqlBigInt;
                    break;
                case Constants.TypeChar:
                    if (arraySize == -1)
                    {
                        dt = DataTypes.SqlVarCharMax;
                    }
                    else if (arrayVariable)
                    {
                        dt = DataTypes.SqlVarChar;
                        dt.Length = arraySize;
                    }
                    else
                    {
                        dt = DataTypes.SqlChar;
                        dt.Length = arraySize;
                    }
                    break;
                case Constants.TypeUnicodeChar:
                    if (arraySize == -1)
                    {
                        dt = DataTypes.SqlNVarCharMax;
                    }
                    else if (arrayVariable)
                    {
                        dt = DataTypes.SqlNVarChar;
                        dt.Length = arraySize;
                    }
                    else
                    {
                        dt = DataTypes.SqlNChar;
                        dt.Length = arraySize;
                    }
                    break;
                case Constants.TypeFloat:
                    dt = DataTypes.SqlReal;
                    break;
                case Constants.TypeDouble:
                    dt = DataTypes.SqlFloat;
                    break;
                case Constants.TypeFloatComplex:
                case Constants.TypeDoubleComplex:
                default:
                    throw new NotImplementedException();
            }

            if (!dt.HasLength && arraySize > 1)
            {
                // Array, not implemented
                throw new NotImplementedException();
            }

            dt.IsNullable = true;  // *** TODO: implement correct null logic

            return dt;
        }

        /// <summary>
        /// Parses the array size string.
        /// </summary>
        /// <param name="arraySizeString"></param>
        /// <param name="size"></param>
        /// <param name="variable"></param>
        private void GetArrayDimensions(string arraySizeString, out int size, out bool variable)
        {
            size = 1;
            variable = false;

            if (!String.IsNullOrEmpty(arraySizeString))
            {
                variable = arraySizeString.Contains('*');

                if (!int.TryParse(arraySizeString.Replace("*", ""), out size))
                {
                    size = -1;
                }
            }
        }

        /// <summary>
        /// Returns column meta data as read from the current field tag on
        /// the xml reader stream.
        /// </summary>
        /// <returns></returns>
        private VariableMetadata GetVOTableMetaData()
        {
            // *** TODO fill in additional column properties
            // read metadata
            //arraysize
            //datatype="char" arraysize="*"/>
            // width
            // precision
            // xtype
            // unit
            // ucd
            // utype
            // ref
            // type

            return null;
        }

        #endregion
#endif
        #region Framework writer functions

        /// <summary>
        /// Writes the resource header into the stream.
        /// </summary>
        protected override async Task OnWriteHeaderAsync()
        {
            await File.XmlWriter.WriteStartElementAsync(null, Constants.TagResource, null);
            await File.XmlWriter.WriteStartElementAsync(null, Constants.TagTable, null);

            // Write columns
            for (int i = 0; i < Columns.Count; i++)
            {
                await WriteColumnAsync(Columns[i]);
            }

            await File.XmlWriter.WriteStartElementAsync(null, Constants.TagData, null);
            await File.XmlWriter.WriteStartElementAsync(null, Constants.TagTableData, null);
        }

        private async Task WriteColumnAsync(Column column)
        {
            await File.XmlWriter.WriteStartElementAsync(null, Constants.TagField, null);

            await File.XmlWriter.WriteAttributeStringAsync(null, Constants.AttributeName, null, column.Name);
            // *** TODO: write other column properties

            await File.XmlWriter.WriteEndElementAsync();
        }

        /// <summary>
        /// Writes the next row into the stream.
        /// </summary>
        /// <param name="values"></param>
        protected override async Task OnWriteNextRowAsync(object[] values)
        {
            await File.XmlWriter.WriteStartElementAsync(null, Constants.TagTR, null);

            for (int i = 0; i < Columns.Count; i++)
            {
                // TODO: Do not use format here, or use standard votable formatting
                if (values[i] == DBNull.Value)
                {
                    // TODO: how to handle nulls in VOTable?
                    // Leave field blank
                }
                else
                {
                    await File.XmlWriter.WriteElementStringAsync(null, Constants.TagTD, null, ColumnFormatters[i](values[i], "{0}"));
                }
            }

            await File.XmlWriter.WriteEndElementAsync();
        }

        /// <summary>
        /// Writers the resource footer into the stream.
        /// </summary>
        protected override async Task OnWriteFooterAsync()
        {
            await File.XmlWriter.WriteEndElementAsync();
            await File.XmlWriter.WriteEndElementAsync();
            await File.XmlWriter.WriteEndElementAsync();
            await File.XmlWriter.WriteEndElementAsync();
        }

        #endregion
    }
}