using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Format.VOTable.V1_3
{
    [XmlRoot(Constants.TagFieldRef, Namespace = Constants.VOTableNamespaceV1_3)]
    public class FieldRef : V1_2.FieldRef
    {
        /*
        [XmlAttribute(Constants.AttributeRef)]
        public string Ref { get; set; }

        [XmlAttribute(Constants.AttributeUcd)]
        public string Ucd { get; set; }

        [XmlAttribute(Constants.AttributeUType)]
        public string UType { set; get; }
        */
    }
}
