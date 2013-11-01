using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Jhu.Graywulf.Types;
using Jhu.Graywulf.Format;

namespace Jhu.SkyQuery.Format.VOTable
{
    public class VOTableResource : FormattedDataFileBlock
    {
        private VOTable File
        {
            get { return (VOTable)file; }
        }

        public VOTableResource(VOTable file)
            : base(file)
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
        }

        #region Column functions

        private void DetectColumns()
        {
            // Read a series for FIELD tags
            // FIELD tags can be combined into GROUP tags

            var cols = new List<DataFileColumn>();

            while (true)
            {
                if (File.XmlReader.NodeType == XmlNodeType.Element &&
                    VOTable.Comparer.Compare(File.XmlReader.Name, VOTableKeywords.Field) == 0)
                {
                    // Column found

                    var col = new DataFileColumn();

                    col.Name = File.XmlReader.GetAttribute(VOTableKeywords.Name);

                    var datatype = File.XmlReader.GetAttribute(VOTableKeywords.DataType);
                    var arraysize = File.XmlReader.GetAttribute(VOTableKeywords.ArraySize);

                    col.DataType = GetVOTableDataType(datatype, arraysize);

                    // *** TODO fill in additional column properties
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

                    col.IsNullable = true;  // *** TODO: implement correct null logic
                    
                    // read metadata

                    cols.Add(col);

                    File.XmlReader.Read();
                }
                else if (VOTable.Comparer.Compare(File.XmlReader.Name, VOTableKeywords.Data) == 0)
                {
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

        private DataType GetVOTableDataType(string voTableType, string arraySizeString)
        {
            int arraySize;
            bool arrayVariable;
            GetArrayDimensions(arraySizeString, out arraySize, out arrayVariable);

            // TODO: implement arrays
            DataType dt;

            switch (voTableType.ToLower(System.Globalization.CultureInfo.InvariantCulture))
            {
                case "boolean":
                    dt = DataType.Boolean;
                    break;
                case "bit":
                    dt = DataType.Boolean;    // This is union bit
                    break;
                case "unsignedbyte":
                    if (arraySize == -1)
                    {
                        dt = DataType.SqlVarBinaryMax;
                    }
                    else if (arrayVariable)
                    {
                        dt = DataType.SqlVarBinary;
                        dt.Length = arraySize;
                    }
                    else
                    {
                        dt = DataType.SqlBinary;
                        dt.Length = arraySize;
                    }
                    break;
                case "short":
                    dt = DataType.SqlSmallInt;
                    break;
                case "int":
                    dt = DataType.SqlInt;
                    break;
                case "long":
                    dt = DataType.SqlBigInt;
                    break;
                case "char":
                    if (arraySize == -1)
                    {
                        dt = DataType.SqlVarCharMax;
                    }
                    else if (arrayVariable)
                    {
                        dt = DataType.SqlVarChar;
                        dt.Length = arraySize;
                    }
                    else
                    {
                        dt = DataType.SqlChar;
                        dt.Length = arraySize;
                    }
                    break;
                case "unicodechar":
                    if (arraySize == -1)
                    {
                        dt = DataType.SqlNVarCharMax;
                    }
                    else if (arrayVariable)
                    {
                        dt = DataType.SqlNVarChar;
                        dt.Length = arraySize;
                    }
                    else
                    {
                        dt = DataType.SqlNChar;
                        dt.Length = arraySize;
                    }
                    break;
                case "float":
                    dt = DataType.SqlReal;
                    break;
                case "double":
                    dt = DataType.SqlFloat;
                    break;
                case "floatcomplex":
                case "doublecomplex":
                default:
                    throw new NotImplementedException();
            }

            if (!dt.HasLength && arraySize > 1)
            {
                // Array, not implemented
                throw new NotImplementedException();
            }

            return dt;
        }

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

        #endregion

        protected override void OnReadHeader()
        {
            // Reader must now be positioned on a TABLE tag
            File.XmlReader.ReadStartElement(VOTableKeywords.Table);

            DetectColumns();
            
            // Consume beginning tags: Data and TableData
            File.XmlReader.ReadStartElement(VOTableKeywords.Data);
            File.XmlReader.ReadStartElement(VOTableKeywords.TableData);
            
            // Reader is positioned on the first TR tag now
        }

        protected override bool ReadNextRowParts(out string[] parts, bool skipComments)
        {
            parts = new string[Columns.Count];

            if (File.XmlReader.NodeType == XmlNodeType.EndElement &&
                (VOTable.Comparer.Compare(File.XmlReader.Name, VOTableKeywords.TableData) == 0 ||
                 VOTable.Comparer.Compare(File.XmlReader.Name, VOTableKeywords.Data) == 0))
            {
                File.XmlReader.ReadEndElement();

                // End of table
                return false;
            }
            else
            {
                // Consume TR tag
                File.XmlReader.ReadStartElement(VOTableKeywords.TR);

                // Read the TD tags
                int q = 0;
                while (true)
                {
                    if (File.XmlReader.NodeType == XmlNodeType.Element &&
                        VOTable.Comparer.Compare(File.XmlReader.Name, VOTableKeywords.TD) == 0)
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
                        VOTable.Comparer.Compare(File.XmlReader.Name, VOTableKeywords.TR) == 0)
                    {
                        // End of a row found
                        File.XmlReader.ReadEndElement();

                        break;
                    }
                    else
                    {
                        throw new FormatException();    // *** TODO
                    }
                }

                return true;
            }
        }

        protected override void OnReadToFinish()
        {
            // If the current element is not a /TABLE, read until the next one
            while (File.XmlReader.NodeType != XmlNodeType.EndElement ||
                VOTable.Comparer.Compare(File.XmlReader.Name, VOTableKeywords.Table) != 0)
            {
                File.XmlReader.Read();
            }

            // Consume closeing tag
            File.XmlReader.ReadEndElement();
        }

        protected override void OnReadFooter()
        {
            // Tags to consume: /RESOURCE
            File.XmlReader.ReadEndElement();
        }

        protected override void OnWriteHeader()
        {
            throw new NotImplementedException();
        }

        protected override void OnWriteNextRow(object[] values)
        {
            throw new NotImplementedException();
        }

        protected override void OnWriteFooter()
        {
            throw new NotImplementedException();
        }
    }
}
