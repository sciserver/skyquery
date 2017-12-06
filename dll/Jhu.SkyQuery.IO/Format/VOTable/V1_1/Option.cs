using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Format.VOTable.V1_1
{
    [XmlRoot(Constants.TagOption, Namespace = Constants.VOTableNamespaceV1_1)]
    public class Option
    {
        [XmlAttribute(Constants.AttributeName)]
        public string Name { get; set; }

        [XmlAttribute(Constants.AttributeValue)]
        public string Value { get; set; }

        [XmlElement(Constants.TagOption)]
        public Option[] Options { get; set; }
    }
}
