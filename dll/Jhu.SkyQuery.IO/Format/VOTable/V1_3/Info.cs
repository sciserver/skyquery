using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Format.VOTable.V1_3
{
    [XmlRoot(Constants.TagInfo, Namespace = Constants.VOTableNamespaceV1_3)]
    public class Info : V1_2.Info
    {
        /*
        [XmlText]
        public string Text { get; set; }

        [XmlAttribute(Constants.AttributeID)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeName)]
        public string Name { get; set; }

        [XmlAttribute(Constants.AttributeValue)]
        public string Value { get; set; }

        [XmlAttribute(Constants.AttributeUnit)]
        public string Unit { get; set; }

        [XmlAttribute(Constants.AttributeXType)]
        public string XType { get; set; }

        [XmlAttribute(Constants.AttributeRef)]
        public string Ref { get; set; }

        [XmlAttribute(Constants.AttributeUcd)]
        public string Ucd { get; set; }

        [XmlAttribute(Constants.AttributeUType)]
        public string UType { get; set; }
        */
    }
}
