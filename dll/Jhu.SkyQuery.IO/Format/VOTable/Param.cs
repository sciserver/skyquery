using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.Format.VOTable
{
    [XmlRoot(Constants.TagParam)]
    public class Param
    {
        // extension Field

        [XmlAttribute(Constants.AttributeValue)]
        public string Value { get; set; }
    }
}
