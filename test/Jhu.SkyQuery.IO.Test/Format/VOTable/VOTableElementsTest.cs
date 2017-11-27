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
        public void ReadCoosysElementTest()
        {
            var xml = "<COOSYS ID='coosys_FK5' system='eq_FK5' equinox='2000.0'/>";
            var e = ReadElementHelper<Coosys>(xml);

            Assert.AreEqual("coosys_FK5", e.ID);
            Assert.AreEqual("eq_FK5", e.System);
            Assert.AreEqual("2000.0", e.Equinox);
            // Need check
        }

        [TestMethod]
        public void ReadDefinitionsElementTest()
        {
            var xml = "";
            var e = ReadElementHelper<Definitions>(xml);

            //To do...
        }


        [TestMethod]
        public void ReadDescriptionElementTest()
        {
            var xml = "<DESCRIPTION> Velocities and Distance estimations </DESCRIPTION>";
            var e = ReadElementHelper<Description>(xml);

            Assert.AreEqual(" Velocities and Distance estimations ", e.Text);
        }

        [TestMethod]
        public void ReadFieldElementTest()
        {
            var xml = "<FIELD ID=\"ID\" unit=\"un\" datatype=\"dt\" precision=\"pr\" width=\"wi\" xtype=\"xt\" ref=\"re\" name=\"na\" ucd=\"uc\" utype=\"ut\" arraysize=\"as\" type=\"ty\"/> ";
            var e = ReadElementHelper<Field>(xml);

            Assert.AreEqual("ID", e.ID);
            Assert.AreEqual("un", e.Unit);
            Assert.AreEqual("dt", e.Datatype);
            Assert.AreEqual("pr", e.Precision);
            Assert.AreEqual("wi", e.Width);
            Assert.AreEqual("xt", e.Xtype);
            Assert.AreEqual("re", e.Ref);
            Assert.AreEqual("na", e.Name);
            Assert.AreEqual("uc", e.Ucd);
            Assert.AreEqual("ut", e.Utype);
            Assert.AreEqual("as", e.Arraysize);
            Assert.AreEqual("ty", e.Type);
        }

        [TestMethod]
        public void ReadFieldRefElementTest()
        {
            var xml = "<FIELDref ref= \"field_2\" ucd = \"em.phot.mag\" utype = \"any.thing2\" />";
            var e = ReadElementHelper<FieldRef>(xml);

            Assert.AreEqual("field_2", e.Ref);
            Assert.AreEqual("em.phot.mag", e.Ucd);
            Assert.AreEqual("any.thing2", e.UType);
        }

        [TestMethod]
        public void ReadGroupElementTest()
        {
            var xml = " <GROUP ID=\"link1\" name=\"group_name\" ref=\"???\" ucd=\"em.phot\" utype=\"???\"></GROUP> ";
            var e = ReadElementHelper<Group>(xml);

            Assert.AreEqual("link1", e.ID);
            Assert.AreEqual("group_name", e.Name);
            Assert.AreEqual("???", e.Ref);
            Assert.AreEqual("em.phot", e.Ucd);
            Assert.AreEqual("???", e.UType);
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

        [TestMethod]
        public void ReadLinkElementTest()
        {
            var xml = "<LINK ID=\"link1\" content-role=\"location\" content-type=\"text/plain\" title=\"Another representation\" value=\"whatever value\" href=\"http://voservices.net/skyquery\" gref=\"???\" action=\"http://voservices.net/skyquery\"/> ";
            var e = ReadElementHelper<Link>(xml);

            Assert.AreEqual("link1", e.ID);
            Assert.AreEqual("location", e.ContentRole);
            Assert.AreEqual("text/plain", e.ContentType);
            Assert.AreEqual("Another representation", e.Title);
            Assert.AreEqual("whatever value", e.Value);
            Assert.AreEqual("http://voservices.net/skyquery", e.Href);
            Assert.AreEqual("???", e.Gref);
            Assert.AreEqual("http://voservices.net/skyquery", e.Action);
        }

        [TestMethod]
        public void ReadMaxElementTest()
        {
            var xml = "<MAX value=\"0.32\" inclusive=\"yes\" />";
            var e = ReadElementHelper<Max>(xml);

            Assert.AreEqual("0.32", e.Value);
            Assert.AreEqual("yes", e.Inclusive);
        }

        [TestMethod]
        public void ReadMinElementTest()
        {
            var xml = "<MIN value=\"0\" inclusive=\"yes\" />";
            var e = ReadElementHelper<Min>(xml);

            Assert.AreEqual("0", e.Value);
            Assert.AreEqual("yes", e.Inclusive);
        }


        [TestMethod]
        public void ReadOptionElementTest()
        {
            var xml = "<OPTION name=\"na\" value=\"va\" />";
            var e = ReadElementHelper<Option>(xml);

            Assert.AreEqual("na", e.Name);
            Assert.AreEqual("va", e.Value);
        }

        [TestMethod]
        public void ReadParamElementTest()
        {
            var xml = "<PARAM value=\"va\"/>";
            var e = ReadElementHelper<Param>(xml);

            Assert.AreEqual("va", e.Value);
        }

        [TestMethod]
        public void ReadParamRefElementTest()
        {
            var xml = "<PARAMref ref=\"re\" ucd=\"uc\" utype=\"ut\"/>";
            var e = ReadElementHelper<ParamRef>(xml);

            Assert.AreEqual("re", e.Ref);
            Assert.AreEqual("uc", e.Ucd);
            Assert.AreEqual("ut", e.UType);
        }
    }
}
