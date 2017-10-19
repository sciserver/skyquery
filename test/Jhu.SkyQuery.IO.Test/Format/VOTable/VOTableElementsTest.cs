using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.SkyQuery.Format.VOTable
{
    [TestClass]
    public class VOTableElementsTest
    {
        [TestMethod]
        public void DeserializerTest()
        {
            // The streaming reader cannot deserialize the entire
            // xml document into memory but the serializer can work
            // from a position and read a single tag (and its contents)

            var xml = @"
<VOTABLE>
    <DESCRIPTION>This is just simple text or can contain <b>embeded HTML</b></DESCRIPTION>
</VOTABLE>";

            var r = XmlReader.Create(new StringReader(xml));
            r.ReadStartElement("VOTABLE");

            var s = new XmlSerializer(typeof(Description));
            s.Deserialize(r);

            r.ReadEndElement();

            Assert.IsTrue(r.EOF);
        }

        private T ReadElementHelper<T>(string xml)
        {
            var s = new XmlSerializer(typeof(T));
            var r = XmlReader.Create(new StringReader(xml));

            return (T)s.Deserialize(r);
        }

        [TestMethod]
        public void ReadDescriptionElementTest()
        {
            var xml = "<DESCRIPTION>This is just simple text or can contain <b>embeded HTML</b></DESCRIPTION>";
            var e = ReadElementHelper<Description>(xml);

            Assert.AreEqual("This is just simple text or can contain <b>embeded HTML</b>", e.Text);
        }

        [TestMethod]
        public void ReadInfoElementTest()
        {
            var xml = "<INFO name=\"info_tag\" value=\"0\">inner text</INFO>";
            var e = ReadElementHelper<Info>(xml);

            Assert.AreEqual("info_tag", e.Name);
            Assert.AreEqual("0", e.Value);
            Assert.AreEqual("inner text", e.Text);
        }


    }
}
