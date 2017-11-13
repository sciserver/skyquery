using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Format.VOTable
{
    [XmlRoot(Constants.TagDefinitions)]
    public class Definitions
    {
        [XmlElement(Constants.TagCoosys)]
        public Coosys Coosys { get; set; }

        [XmlElement(Constants.TagParam)]
        public Param Param { get; set; }

    }
}
