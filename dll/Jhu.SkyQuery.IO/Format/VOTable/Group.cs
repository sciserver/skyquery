using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Format.VOTable
{
    [XmlRoot(Constants.TagGroup)]
    public class Group
    {
        [XmlElement(Constants.TagDescription)]
        public Description Description { get; set; }

        [XmlElement(Constants.TagFieldRef)]
        public FieldRef FieldRef { get; set; }

        [XmlElement(Constants.TagParamRef)]
        public ParamRef ParamRef { get; set; }

        [XmlElement(Constants.TagParam)]
        public Param Param { get; set; }

        [XmlElement(Constants.TagGroup)]
        public Group[] Groups { get; set; }

        [XmlAttribute(Constants.AttributeID)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeName)]
        public string Name { get; set; }

        [XmlAttribute(Constants.AttributeRef)]
        public string Ref { get; set; }

        [XmlAttribute(Constants.AttributeUcd)]
        public string Ucd { get; set; }

        [XmlAttribute(Constants.AttributeUType)]
        public string UType { get; set; }
    }
}
