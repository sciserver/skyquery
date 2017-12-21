using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Jhu.Graywulf.Schema;

namespace Jhu.SkyQuery.Format.VOTable.V1_1
{
    [XmlRoot(ElementName = Constants.TagField, Namespace = Constants.VOTableNamespaceV1_1)]
    public class Field
    {
        [XmlElement(Constants.TagDescription)]
        public Description Description { get; set; }

        [XmlElement(Constants.TagValues)]
        public Values Values { get; set; }

        [XmlElement(Constants.TagLink)]
        public Link Link { get; set; }

        [XmlAttribute(Constants.AttributeID)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeUnit)]
        public string Unit { get; set; }

        [XmlAttribute(Constants.AttributeDatatype)]
        public string Datatype { get; set; }

        [XmlAttribute(Constants.AttributePrecision)]
        public string Precision { get; set; }

        [XmlAttribute(Constants.AttributeWidth)]
        public string Width { get; set; }

        [XmlAttribute(Constants.AttributeRef)]
        public string Ref { get; set; }

        [XmlAttribute(Constants.AttributeName)]
        public string Name { get; set; }

        [XmlAttribute(Constants.AttributeUcd)]
        public string Ucd { get; set; }

        [XmlAttribute(Constants.AttributeUType)]
        public string Utype { get; set; }

        [XmlAttribute(Constants.AttributeArraySize)]
        public string Arraysize { get; set; }

        [XmlAttribute(Constants.AttributeType)]
        public string Type { get; set; }

        private void GetArraySize(out int[] size, out bool isArray, out bool isVariableSize, out bool isUnboundSize)
        {
            // example: <FIELD ID= "values" datatype="int" arraysize="100*"/>
            // example: <FIELD ID= "values" datatype="int" arraysize="100,*"/>
            // example: <FIELD ID= "values" datatype="int" arraysize="100,10*"/>

            if (String.IsNullOrEmpty(Arraysize))
            {
                size = new int[0];
                isArray = false;
                isVariableSize = false;
                isUnboundSize = false;
            }
            else
            {
                isArray = true;

                var parts = Arraysize.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var lastpart = parts[parts.Length - 1];

                if (lastpart == "*")
                {
                    // "10,20,*"
                    isVariableSize = false;
                    isUnboundSize = true;
                    size = new int[parts.Length - 1];
                }
                else if (lastpart.EndsWith("*"))
                {
                    // "10,20*"
                    isVariableSize = true;
                    isUnboundSize = false;
                    size = new int[parts.Length];
                }
                else
                {
                    // "10,20"
                    isVariableSize = false;
                    isUnboundSize = false;
                    size = new int[parts.Length];
                }

                for (int i = 0; i < size.Length; i++)
                {
                    size[i] = Int32.Parse(parts[i].TrimEnd('*'));
                }
            }
        }

        public virtual Column CreateColumn()
        {
            Column c;
            GetArraySize(out int[] size, out bool isArray, out bool isVariableSize, out bool isUnboundSize);
            
            switch (Datatype)
            {
                case Constants.TypeBoolean:
                    c = new Column(Name, DataTypes.SqlBit);
                    break;
                case Constants.TypeBit:
                    throw Error.BitNotSupported();
                case Constants.TypeByte:
                    c = new Column(Name, DataTypes.Byte);
                    break;
                case Constants.TypeShort:
                    c = new Column(Name, DataTypes.SqlSmallInt);
                    break;
                case Constants.TypeInt:
                    c = new Column(Name, DataTypes.SqlInt);
                    break;
                case Constants.TypeLong:
                    c = new Column(Name, DataTypes.SqlBigInt);
                    break;
                case Constants.TypeFloat:
                    c = new Column(Name, DataTypes.SqlReal);
                    break;
                case Constants.TypeDouble:
                    c = new Column(Name, DataTypes.SqlFloat);
                    break;
                case Constants.TypeFloatComplex:
                    c = new Column(Name, DataTypes.SingleComplex);
                    break;
                case Constants.TypeDoubleComplex:
                    c = new Column(Name, DataTypes.DoubleComplex);
                    break;
                case Constants.TypeChar:
                    if (!isArray)
                    {
                        // Single character
                        c = new Column(Name, DataTypes.SqlChar);
                    }
                    else if (size.Length == 0 && isUnboundSize)
                    {
                        // Unbound length
                        c = new Column(Name, DataTypes.SqlVarCharMax);
                    }
                    else if (size.Length == 1 && isVariableSize)
                    {
                        // Variable length
                        c = new Column(Name, DataTypes.SqlVarChar);
                    }
                    else if (size.Length == 1 && !isVariableSize)
                    {
                        // Fixed length
                        c = new Column(Name, DataTypes.SqlChar);
                    }
                    else
                    {
                        throw Error.MultidimensionalStringNotSupported();
                    }
                    break;
                case Constants.TypeUnicodeChar:
                    if (!isArray)
                    {
                        // Single character
                        c = new Column(Name, DataTypes.SqlNChar);
                    }
                    else if (size.Length == 0 && isUnboundSize)
                    {
                        // Unbound length
                        c = new Column(Name, DataTypes.SqlNVarCharMax);
                    }
                    else if (size.Length == 1 && isVariableSize)
                    {
                        // Variable length
                        c = new Column(Name, DataTypes.SqlNVarChar);
                    }
                    else if (size.Length == 1 && !isVariableSize)
                    {
                        // Fixed length
                        c = new Column(Name, DataTypes.SqlNChar);
                    }
                    else
                    {
                        throw Error.MultidimensionalStringNotSupported();
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            // VOTable data types are nullable by default
            c.DataType.IsNullable = true;

            if (c.DataType.Type != typeof(string) && (isArray || isUnboundSize))
            {
                throw Error.PrimitiveArraysNotSupported();
            }
            else if (c.DataType.Type == typeof(string) && isArray && !isUnboundSize)
            {
                c.DataType.Length = size[0];
            }

            return c;
        }
    }
}
