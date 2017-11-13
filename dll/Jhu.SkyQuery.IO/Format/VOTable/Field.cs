using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Format.VOTable
{
    [XmlRoot(ElementName = Constants.TagField, Namespace = Constants.VOTableNamespace)]
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

        [XmlAttribute(Constants.AttributeXType)]
        public string Xtype { get; set; }

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
    }
}
