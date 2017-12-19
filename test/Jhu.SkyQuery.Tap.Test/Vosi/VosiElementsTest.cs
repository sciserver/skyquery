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

namespace Jhu.SkyQuery.Vosi
{
    [TestClass]
    public class VosiElementsTest
    {
        private T ReadElementHelper<T>(string xml)
        {
            var s = new XmlSerializer(typeof(T));
            var r = XmlReader.Create(new StringReader(xml));

            return (T)s.Deserialize(r);
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
            var e = ReadElementHelper<Vosi.Availability.V1_0.Availability>(xml);

            Assert.IsTrue(e.Available);
        }
    }
}
