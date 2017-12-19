using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.VoResource.V1_0
{
    [XmlRoot(Constants.TagCapability, Namespace = Constants.VoResourceNamespaceV1_0)]
    public class Capability
    {
        public Validation[] ValidationLevelList { get; set; }
        public string Description { get; set; }
        public Interface[] InterfaceList { get; set; }
        public string StandardID { get; set; }
    }
}
