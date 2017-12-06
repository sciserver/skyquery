using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Format.VOTable.V1_3
{
    [XmlRoot(Constants.TagCoosys, Namespace = Constants.VOTableNamespaceV1_3)]
    public class Coosys : V1_2.Coosys
    {
        /*
        [XmlAttribute(Constants.AttributeID)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeSystem)]
        public string System { get; set; }

        [XmlAttribute(Constants.AttributeEquinox)]
        public string Equinox { get; set; }
        */
    }
}
