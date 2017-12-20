using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Test;

namespace Jhu.SkyQuery.Vosi
{
    [TestClass]
    public class VosiElementsTest : TestClassBase
    {
        private T ReadElementHelper<T>(string xml)
        {
            var s = new XmlSerializer(typeof(T));
            s.UnknownNode += S_UnknownNode;

            var settings = new XmlReaderSettings()
            {
                
            };
            var r = XmlReader.Create(new StringReader(xml));
            return (T)s.Deserialize(r);
        }

        private void S_UnknownNode(object sender, XmlNodeEventArgs e)
        {
        }
        
        [TestMethod]
        public void AvailabilityTest()
        {
            var xml = 
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<availability xmlns=""http://www.ivoa.net/xml/VOSIAvailability/v1.0"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.ivoa.net/xml/VOSIAvailability/v1.0 http://www.ivoa.net/xml/VOSIAvailability/v1.0"">
        <available>true</available>
        <note>VizieR TAP service available </note>
</availability>";
            var e = ReadElementHelper<VO.Vosi.Availability.V1_0.Availability>(xml);

            Assert.IsTrue(e.Available);
        }

        [TestMethod]
        public void CapabilitiesTest()
        {
            string path = GetTestFilePath(@"modules\skyquery\test\files\tap_vosi\capabilities.xml");
            var xml = File.ReadAllText(path);
            var e = ReadElementHelper<VO.Vosi.Capabilities.V1_0.Capabilities>(xml);
        }
    }
}
