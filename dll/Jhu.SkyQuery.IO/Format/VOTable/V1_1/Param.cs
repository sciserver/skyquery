using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Format.VOTable.V1_1
{
    [XmlRoot(Constants.TagParam, Namespace = Constants.VOTableNamespaceV1_1)]
    public class Param : Field
    {
        // extension Field

        [XmlAttribute(Constants.AttributeValue)]
        public string Value { get; set; }
    }
}
