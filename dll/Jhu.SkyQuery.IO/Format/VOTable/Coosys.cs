using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Format.VOTable
{
    [XmlRoot(Constants.TagCoosys)]
    public class Coosys
    {
        [XmlAttribute(Constants.AttributeID)]
        public string ID { get; set; }

        [XmlAttribute(Constants.AttributeSystem)]
        public string System { get; set; }

        [XmlAttribute(Constants.AttributeEquinox)]
        public string Equinox { get; set; }
    }
}
