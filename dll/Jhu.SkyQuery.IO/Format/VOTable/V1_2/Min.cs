using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Format.VOTable.V1_2
{
    [XmlRoot(Constants.TagMin, Namespace = Constants.VOTableNamespaceV1_2)]
    public class Min : V1_1.Min
    {
        /*
        [XmlAttribute(Constants.AttributeValue)]
        public string Value { get; set; }

        [XmlAttribute(Constants.AttributeInclusive)]
        public string Inclusive { get; set; }
        */
    }
}
