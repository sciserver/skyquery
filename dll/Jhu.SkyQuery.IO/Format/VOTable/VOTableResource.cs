using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Format;
using System.Threading.Tasks;

using System.Xml.Serialization;//Tom6
using System.IO;//Tom6

namespace Jhu.SkyQuery.Format.VOTable
{
    /// <summary>
    /// Implements functionality responsible for reading and writing a single
    /// resource block within a VOTable.
    /// </summary>
    [Serializable]
    public class VOTableResource : XmlDataFileBlock, ICloneable
    {
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
        }

        private void CopyMembers(VOTableResource old)
        {
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
            // TODO: Implement this

            // If the RESOURCE contains a DATATABLE then the string contents
            // of the TD tags can be returned and the XmlDataFileBlock class will
            // take care of the data conversion, so simply call
            return base.OnReadNextRowAsync(values);

            // If the RESOURCE is binary then implement this function, parse
            // columns from byte stream and return them
        }

        /// <summary>
        /// Completes reading of a resource by reading its closing tag.
        /// </summary>
        protected override Task OnReadFooterAsync()
        {
            // TODO: make sure the ending RESOURCE tag is read and the reader
            // is positioned at the next tag

            // TODO: the TABLE element and the RESOURCE element can contain
            // trailing INFO tags (what are these for?)
            // make sure that they are read and position the reader after the
            // closing RESOURCE element, whatever it is.
            if (File.XmlReader.NodeType == XmlNodeType.EndElement &&
                (VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagData) == 0 ||
                (VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTable) == 0)))
            {
                File.XmlReader.ReadEndElement();
                //Info in DATA
                //Just skip
                while(File.XmlReader.NodeType == XmlNodeType.Element && 
                    VOTable.Comparer.Compare(File.XmlReader.Name,Constants.TagInfo) == 0)
                {
                    File.XmlReader.ReadStartElement(Constants.TagInfo);
                    File.XmlReader.Skip();
                    //var xml = File.Deserialize<Info>();

                    File.XmlReader.ReadEndElement();
                }
                //End Table
                //if (File.XmlReader.NodeType == XmlNodeType.EndElement &&
                //    VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTable) == 0)
                //{
                    File.XmlReader.ReadEndElement();
                //}
                //Info on RESOURCE
                //Just skip
                while (File.XmlReader.NodeType == XmlNodeType.Element &&
                   VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagInfo) == 0)
                {
                    File.XmlReader.ReadStartElement(Constants.TagInfo);
                    File.XmlReader.Skip();
                    File.XmlReader.ReadEndElement();
                }
                //End Resourxe
                //if (File.XmlReader.NodeType == XmlNodeType.EndElement &&
                //    VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagResource) == 0)
                //{
                    File.XmlReader.ReadEndElement();
                //}
            }
            else
            {
                throw new FileFormatException();
            }
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
            // TODO: This can be called by the framework anywhere within a RESOURCE tag,
            // and it has to make sure that the reader is position right after the
            // closing RESOURCE tag. This is for skipping or otherwise finishing the
            // file block

            /* OLD CODE, REUSE OR DELETE
            // If the current element is not a /TABLE, read until the next one
            while (File.XmlReader.NodeType != XmlNodeType.EndElement ||
                VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTable) != 0)
            {
                File.XmlReader.Read();
            }

            // Consume closeing tag
            File.XmlReader.ReadEndElement();
            */
            if (VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTableData) != 0)
            {
                while (File.XmlReader.NodeType != XmlNodeType.EndElement ||
                    VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTableData) != 0)
                {
                    File.XmlReader.Read();
                }
            }
            File.XmlReader.ReadEndElement();
        }

        /// <summary>
        /// Returns an array of strings containing data from the next data row.
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="skipComments"></param>
        /// <returns></returns>
        protected override Task<bool> OnReadNextRowPartsAsync(IList<string> parts, bool skipComments)
        {

            // OLD CODE, REUSED 
            parts = new string[Columns.Count];


            if (File.XmlReader.NodeType == XmlNodeType.EndElement &&
                (VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTableData) == 0 ||
                 VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagData) == 0))
            {
             //   File.XmlReader.ReadEndElement();

                // End of table
                return false;
            }
            else
            {

                // Consume TR tag
                File.XmlReader.ReadStartElement(Constants.TagTR);

                // Read the TD tags
                int q = 0;
                while (true)
                {
                    if (File.XmlReader.NodeType == XmlNodeType.Element &&
                        VOTable.Comparer.Compare(File.XmlReader.Name, Constants.TagTD) == 0)
                    {

                        if (!File.XmlReader.IsEmptyElement)
                        {
                            File.XmlReader.ReadStartElement(Constants.TagTD);
                            // A cell found
                            parts[q] = File.XmlReader.ReadString();

                            // Consume closing tag
                            File.XmlReader.ReadEndElement();
                        }
                        else
                        {
                            parts[q] = null;

                            File.XmlReader.Read();
                        }

                        q++;
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
                }

                return true;
            }
        }

        #endregion
        #region VOTable reader functions

        private void ReadResourceElement()
        {

            // The reader is now positioned on the RESOURCE tag            
                File.XmlReader.ReadStartElement(Constants.TagResource);
            // New part 
            // Read all tags inside VOTABLE but stop at any RESOURCE tag
            // because they are handled outside of this function
            int q = 0; // Count the number of FILEDs 

            while (File.XmlReader.NodeType == XmlNodeType.Element &&
                   XmlDataFile.Comparer.Compare(File.XmlReader.Name, Constants.TagData) != 0)
            {
                switch (File.XmlReader.Name)
                {
                    case Constants.TagDescription:
                        File.XmlReader.Skip();
                        break;
                    case Constants.TagInfo:
                        File.XmlReader.Skip();
                        break;
                    case Constants.TagCoosys:
                        File.XmlReader.Skip();
                        break;
                    case Constants.TagGroup:
                        File.XmlReader.Skip();
                        break;
                    case Constants.TagParam:
                        File.XmlReader.Skip();
                        break;
                    case Constants.TagLink:
                        File.XmlReader.Skip();
                        break;
                    case Constants.TagField:
                        File.XmlReader.Skip();
                        q++;
                        break;
                    case Constants.TagTable:
                        ReadTableElement();
                        // TODO: implement deserializets,
                        break;

                    case Constants.TagResource:
                        throw Error.RecursiveResourceNotSupported();
                    default:
                        throw new NotImplementedException();
                }

                File.XmlReader.MoveToContent();
            }

            // TODO: read all possible tags here similary to VOTable.ReadVOTableElement
            // * RESOURCE

            // * TITLE ?

            //OK TODO: while processing the above tags, collect info on columns   

            // Reader must now be positioned on a TABLE tag or an embeded RESOURCE tag
            // TODO: process table
            // TODO: throw exception on embeded resource

            // Consume beginning tags: Data and TableData
            File.XmlReader.ReadStartElement(Constants.TagData);
            File.XmlReader.ReadStartElement(Constants.TagTableData);

            // Reader is positioned on the first TR tag now
        }

        private void ReadTableElement()
        {
            // TODO: this works very similarly to the RESOURCE tag and
            // can contain these tags:
            // * DESCRIPTION
            // * INFO
            // * FIELS
            // * PARAM
            // * GROUP

            // TODO: while processing the above tags, collect info on columns
            var columns = new List<Column>();

            File.XmlReader.ReadStartElement(Constants.TagTable);

            int q = 0;

            while (File.XmlReader.NodeType == XmlNodeType.Element &&
        XmlDataFile.Comparer.Compare(File.XmlReader.Name, Constants.TagData) != 0)
            {
                switch (File.XmlReader.Name)
                {
                    case Constants.TagDescription:
                        File.XmlReader.Skip();
                        break;
                    case Constants.TagInfo:
                        File.XmlReader.Skip();
                        break;
                    case Constants.TagCoosys:
                        File.XmlReader.Skip();
                        break;
                    case Constants.TagGroup:
                        File.XmlReader.Skip();
                        break;
                    case Constants.TagParam:
                        File.XmlReader.Skip();
                        break;
                    case Constants.TagLink:
                        File.XmlReader.Skip();
                        break;
                    case Constants.TagField:
                        var xml = File.Deserialize<Field>();
                        var c = CreateColumn(xml);
                        c.ID = q;
                        q++;
                        columns.Add(c);
                        break;
                }

                File.XmlReader.MoveToContent();
            }

            // TODO: at this point we have all info on columns, so
            // call CreateColumns on base class

            CreateColumns(columns);

            // The reader is now positioned on a LINK or a DATA tag
            // we do not support LINK tags, so throw on error
            // If a data tag is found, process further
        }

        private Column CreateColumn(Field field)
        {
            Column c;

            switch (field.Datatype)
            {
                // TODO: ARRAYS
                // exampla: <FIELD ID= "values" datatype="int" arraysize="100*"/>
                case "boolean":
                    c = new Column(field.Name, DataTypes.SqlBit);
                    break;
                case "bit":
                    c = new Column(field.Name, DataTypes.SqlBit);
                    break;
                case "unsignedByte":
                    c = new Column(field.Name, DataTypes.SqlSmallInt);
                    break;
                case "char":
                    if (field.Arraysize == "*")
                    {
                        c = new Column(field.Name, DataTypes.SqlVarCharMax);
                    }
                    else if (!String.IsNullOrWhiteSpace(field.Arraysize))
                    {
                        c = new Column(field.Name, DataTypes.SqlVarChar);
                        c.DataType.Length = Int32.Parse(field.Arraysize);
                    }
                    else
                    {
                        c = new Column(field.Name, DataTypes.SqlChar);
                    }
                    break;
                case "unicodeChar":
                    c = new Column(field.Name, DataTypes.SqlNChar);
                    break;
                case "short":
                    c = new Column(field.Name, DataTypes.SqlSmallInt);
                    break;
                case "int":
                    c = new Column(field.Name, DataTypes.SqlInt);
                    break;
                case "long":
                    c = new Column(field.Name, DataTypes.SqlBigInt);
                    break;
                case "float":
                    c = new Column(field.Name, DataTypes.SqlReal);
                    break;
                case "double":
                    c = new Column(field.Name, DataTypes.SqlFloat);
                    break;
                case "floatComplex":
                    c = new Column(field.Name, DataTypes.SingleComplex);
                    break;
                case "doubleComplex":
                    c = new Column(field.Name, DataTypes.DoubleComplex);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return c;
        }

        private void ReadDataElement()
        {
            // TODO: The DATA tag can contain one of the following
            // * TABLEDATA
            // * BINARY
            // * BINARY2
        }

        private void ReadTableDataElement()
        {
            // TODO: position reader on the very first TR tag
            // subsequent processing will be done when OnReadNextRow is called
            // by the framework
        }

        private void ReadBinaryElement()
        {
            // TODO: figure out binary type and make sure that OnReadNextRow
            // reads contents accordingly
        }

        private void ReadBinary2Element()
        {
            // TODO: similar to ReadBinaryElement
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

            await File.XmlWriter.WriteAttributeStringAsync(null,Constants.AttributeName, null, column.Name);
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
