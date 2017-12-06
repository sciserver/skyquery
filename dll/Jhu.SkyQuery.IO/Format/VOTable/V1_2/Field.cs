using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Format.VOTable.V1_2
{
    [XmlRoot(ElementName = Constants.TagField, Namespace = Constants.VOTableNamespaceV1_2)]
    public class Field : V1_1.Field
    {
        [XmlAttribute(Constants.AttributeXType)]
        public string Xtype { get; set; }
    }
}
