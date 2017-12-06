using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Format.VOTable.V1_1
{
    [XmlRoot(Constants.TagCoosys, Namespace = Constants.VOTableNamespaceV1_1)]
    public class Coosys
    {
        
        [XmlText]
        public string Text { get; set; }
        
        [XmlAttribute(Constants.AttributeID)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeEquinox)]
        public string Equinox { get; set; }

        [XmlAttribute(Constants.AttributeEpoch)]
        public string Epoch { get; set; }

        [XmlAttribute(Constants.AttributeSystem)]
        public string System { get; set; }

    }
}
