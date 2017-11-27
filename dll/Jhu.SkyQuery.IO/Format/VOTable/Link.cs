using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Format.VOTable
{
    [XmlRoot(Constants.TagLink)]
    public class Link
    {
        [XmlAttribute(Constants.AttributeID)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeContentRole)]
        public string ContentRole { get; set; }

        [XmlAttribute(Constants.AttributeContentType)]
        public string ContentType { get; set; }

        [XmlAttribute(Constants.AttributeTitle)]
        public string Title { get; set; }

        [XmlAttribute(Constants.AttributeValue)]
        public string Value { get; set; }

        [XmlAttribute(Constants.AttributeHref)]
        public string Href { get; set; }

        [XmlAttribute(Constants.AttributeGref)]
        public string Gref { get; set; }

        [XmlAttribute(Constants.AttributeAction)]
        public string Action { get; set; }
    }
}
