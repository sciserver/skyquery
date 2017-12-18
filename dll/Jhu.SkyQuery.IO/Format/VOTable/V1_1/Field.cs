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

        public virtual Column CreateColumn()
        {
            Column c;

            switch (Datatype)
            {
                // TODO: ARRAYS
                // example: <FIELD ID= "values" datatype="int" arraysize="100*"/>
                case "boolean":
                    c = new Column(Name, DataTypes.SqlBit);
                    break;
                case "bit":
                    c = new Column(Name, DataTypes.SqlBit);
                    break;
                case "unsignedByte":
                    c = new Column(Name, DataTypes.SqlSmallInt);
                    break;
                case "char":
                    /*
                    if (Arraysize == "*" )
                    {
                        c = new Column(Name, DataTypes.SqlVarCharMax);
                    }
                    else if (!String.IsNullOrWhiteSpace(Arraysize))
                    {
                        c = new Column(Name, DataTypes.SqlVarChar);
                        c.DataType.Length = Int32.Parse(Arraysize);
                    }
                    else
                    {
                        c = new Column(Name, DataTypes.SqlChar);
                    }
                    break;
                    */
                    if (!String.IsNullOrWhiteSpace(Arraysize))
                    {
                        if (Arraysize == "*" || Arraysize.Contains("*"))
                        {
                            c = new Column(Name, DataTypes.SqlVarCharMax);
                        }
                        else
                        {
                            c = new Column(Name, DataTypes.SqlVarChar);
                            c.DataType.Length = Int32.Parse(Arraysize);
                        }
                    }
                    else
                    {
                        c = new Column(Name, DataTypes.SqlChar);
                    }
                    break;
                case "unicodeChar":
                    c = new Column(Name, DataTypes.SqlNChar);

                    // TODO length
                    break;
                case "short":
                    c = new Column(Name, DataTypes.SqlSmallInt);
                    break;
                case "int":
                    c = new Column(Name, DataTypes.SqlInt);
                    break;
                case "long":
                    c = new Column(Name, DataTypes.SqlBigInt);
                    break;
                case "float":
                    c = new Column(Name, DataTypes.SqlReal);
                    break;
                case "double":
                    c = new Column(Name, DataTypes.SqlFloat);
                    break;
                case "floatComplex":
                    c = new Column(Name, DataTypes.SingleComplex);
                    break;
                case "doubleComplex":
                    c = new Column(Name, DataTypes.DoubleComplex);
                    break;
                default:
                    throw new NotImplementedException();
            }

            // VOTable data types are nullable by default
            c.DataType.IsNullable = true;
            
            return c;
        }
    }
}
