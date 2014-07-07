using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Format;

namespace Jhu.SkyQuery.Format.VOTable
{
    /// <summary>
    /// Implements functionality responsible for reading and writing a single
    /// resource block within a VOTable.
    /// </summary>
    [Serializable]
    public class VOTableResource : FormattedDataFileBlockBase, ICloneable
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
                    VOTable.Comparer.Compare(File.XmlReader.Name, Constants.VOTableKeywordField) == 0)
                {
                    // Column found

                    var col = new Column()
                    {
                        Name = File.XmlReader.GetAttribute(Constants.VOTableKeywordName),
                        DataType = GetVOTableDataType(),
                        Metadata = GetVOTableMetaData(),
                    };

                    cols.Add(col);

                    File.XmlReader.Read();
                }
                else if (VOTable.Comparer.Compare(File.XmlReader.Name, Constants.VOTableKeywordData) == 0)
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
            var datatype = File.XmlReader.GetAttribute(Constants.VOTableKeywordDataType);
            var arraysize = File.XmlReader.GetAttribute(Constants.VOTableKeywordArraySize);

            int arraySize;
            bool arrayVariable;
            GetArrayDimensions(arraysize, out arraySize, out arrayVariable);

            // TODO: implement arrays
            // TODO: use constants for data type names
            DataType dt;

            switch (datatype.ToLowerInvariant())
            {
                case Constants.VOTableTypeBoolean:
                    dt = DataTypes.Boolean;
                    break;
                case Constants.VOTableTypeBit:
                    dt = DataTypes.Boolean;    // This is union bit
                    break;
                case Constants.VOTableTypeByte:
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
                case Constants.VOTableTypeShort:
                    dt = DataTypes.SqlSmallInt;
                    break;
                case Constants.VOTableTypeInt:
                    dt = DataTypes.SqlInt;
                    break;
                case Constants.VOTableTypeLong:
                    dt = DataTypes.SqlBigInt;
                    break;
                case Constants.VOTableTypeChar:
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
                case Constants.VOTableTypeUnicodeChar:
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
                case Constants.VOTableTypeFloat:
                    dt = DataTypes.SqlReal;
                    break;
                case Constants.VOTableTypeDouble:
                    dt = DataTypes.SqlFloat;
                    break;
                case Constants.VOTableTypeFloatComplex:
                case Constants.VOTableTypeDoubleComplex:
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

        /// <summary>
        /// Reads the header of VOTable resource, from the TABLE tag to the TABLEDATA tag.
        /// </summary>
        protected override void OnReadHeader()
        {
            // Reader must now be positioned on a TABLE tag
            File.XmlReader.ReadStartElement(Constants.VOTableKeywordTable);

            DetectColumns();
            
            // Consume beginning tags: Data and TableData
            File.XmlReader.ReadStartElement(Constants.VOTableKeywordData);
            File.XmlReader.ReadStartElement(Constants.VOTableKeywordTableData);
            
            // Reader is positioned on the first TR tag now
        }

        /// <summary>
        /// Returns an array of strings containing data from the next data row.
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="skipComments"></param>
        /// <returns></returns>
        protected override bool ReadNextRowParts(out string[] parts, bool skipComments)
        {
            parts = new string[Columns.Count];

            if (File.XmlReader.NodeType == XmlNodeType.EndElement &&
                (VOTable.Comparer.Compare(File.XmlReader.Name, Constants.VOTableKeywordTableData) == 0 ||
                 VOTable.Comparer.Compare(File.XmlReader.Name, Constants.VOTableKeywordData) == 0))
            {
                File.XmlReader.ReadEndElement();

                // End of table
                return false;
            }
            else
            {
                // Consume TR tag
                File.XmlReader.ReadStartElement(Constants.VOTableKeywordTR);

                // Read the TD tags
                int q = 0;
                while (true)
                {
                    if (File.XmlReader.NodeType == XmlNodeType.Element &&
                        VOTable.Comparer.Compare(File.XmlReader.Name, Constants.VOTableKeywordTD) == 0)
                    {
                        if (!File.XmlReader.IsEmptyElement)
                        {
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
                        VOTable.Comparer.Compare(File.XmlReader.Name, Constants.VOTableKeywordTR) == 0)
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

        /// <summary>
        /// Completes reading of a table and stops on the last tag.
        /// </summary>
        /// <remarks>
        /// This function is called by the infrastructure to read all possible data
        /// rows that the client didn't consume.
        /// </remarks>
        protected override void OnReadToFinish()
        {
            // If the current element is not a /TABLE, read until the next one
            while (File.XmlReader.NodeType != XmlNodeType.EndElement ||
                VOTable.Comparer.Compare(File.XmlReader.Name, Constants.VOTableKeywordTable) != 0)
            {
                File.XmlReader.Read();
            }

            // Consume closeing tag
            File.XmlReader.ReadEndElement();
        }

        /// <summary>
        /// Completes reading of a resource by reading its closing tag.
        /// </summary>
        protected override void OnReadFooter()
        {
            // Tags to consume: /RESOURCE
            File.XmlReader.ReadEndElement();
        }

        /// <summary>
        /// Writes the resource header into the stream.
        /// </summary>
        protected override void OnWriteHeader()
        {
            File.XmlWriter.WriteStartElement(Constants.VOTableKeywordResource);
            File.XmlWriter.WriteStartElement(Constants.VOTableKeywordTable);

            // Write columns
            for (int i = 0; i < Columns.Count; i++)
            {
                WriteColumn(Columns[i]);
            }

            File.XmlWriter.WriteStartElement(Constants.VOTableKeywordData);
            File.XmlWriter.WriteStartElement(Constants.VOTableKeywordTableData);
        }

        private void WriteColumn(Column column)
        {
            File.XmlWriter.WriteStartElement(Constants.VOTableKeywordField);

            File.XmlWriter.WriteAttributeString(Constants.VOTableKeywordName, column.Name);
            // *** TODO: write other column properties

            File.XmlWriter.WriteEndElement();
        }

        /// <summary>
        /// Writes the next row into the stream.
        /// </summary>
        /// <param name="values"></param>
        protected override void OnWriteNextRow(object[] values)
        {
            File.XmlWriter.WriteStartElement(Constants.VOTableKeywordTR);
            
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
                    File.XmlWriter.WriteElementString(Constants.VOTableKeywordTD, ColumnFormatters[i](values[i], "{0}"));
                }
            }
            
            File.XmlWriter.WriteEndElement();
        }

        /// <summary>
        /// Writers the resource footer into the stream.
        /// </summary>
        protected override void OnWriteFooter()
        {
            File.XmlWriter.WriteEndElement();
            File.XmlWriter.WriteEndElement();
            File.XmlWriter.WriteEndElement();
            File.XmlWriter.WriteEndElement();
        }
    }
}
