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

        [TestMethod]
        public void CapabilitiesTest()
        {
            var xml =
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<vosi:capabilities xmlns:vosi=""http://www.ivoa.net/xml/VOSICapabilities/v1.0"" xmlns:tr=""http://www.ivoa.net/xml/TAPRegExt/v1.0"" xmlns:vr=""http://www.ivoa.net/xml/VOResource/v1.0"" xmlns:vs=""http://www.ivoa.net/xml/VODataService/v1.0"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.ivoa.net/xml/VOSICapabilities/v1.0 http://www.ivoa.net/xml/VOSICapabilities/v1.0 http://www.ivoa.net/xml/TAPRegExt/v1.0 http://www.ivoa.net/xml/TAPRegExt/v1.0 http://www.ivoa.net/xml/VOResource/v1.0 http://www.ivoa.net/xml/VOResource/v1.0 http://www.ivoa.net/xml/VODataService/v1.0 http://www.ivoa.net/xml/VODataService/v1.0"">
<capability  standardID=""ivo://ivoa.net/std/TAP"" xsi:type=""tr:TableAccess"">
        <interface role=""std"" xsi:type=""vs:ParamHTTP"">
                <accessURL use=""base"">http://localhost/TAPVizieR/tap</accessURL>
        </interface>
        <language>
                <name>ADQL</name>
                <version ivo-id=""ivo://ivoa.net/std/ADQL#v2.0"">2.0</version>
                <description>ADQL 2.0</description>
                <languageFeatures type=""ivo://ivoa.net/std/TAPRegExt#features-udf"">                     <feature>                               <form>ROUND(value DOUBLE, prec INTEGER) -&gt; DOUBLE</form>                     </feature>                      <feature>                               <form>CAST(value DOUBLE, cast type VARCHAR) -&gt; DOUBLE</form>                 </feature>                      <feature>                               <form>HEALPIX(ra DOUBLE, dec DOUBLE, order INTEGER) -&gt; INTEGER</form>                        </feature>              </languageFeatures>     </language>
        <outputFormat>
                <mime>text/xml</mime>
                <alias>votable</alias>
        </outputFormat>
        <outputFormat>
                <mime>application/fits</mime>
                <alias>fits</alias>
        </outputFormat>
        <outputFormat>
                <mime>application/json</mime>
                <alias>json</alias>
        </outputFormat>
        <outputFormat>
                <mime>text/csv</mime>
                <alias>csv</alias>
        </outputFormat>
        <outputFormat>
                <mime>text/tab-separated-values</mime>
                <alias>tsv</alias>
        </outputFormat>
        <outputFormat>
                <mime>text/plain</mime>
                <alias>text</alias>
        </outputFormat>
        <outputFormat>
                <mime>text/html</mime>
                <alias>html</alias>
        </outputFormat>
        <uploadMethod ivo-id=""ivo://ivoa.net/std/TAPRegExt#upload-inline"" />
        <uploadMethod ivo-id=""ivo://ivoa.net/std/TAPRegExt#upload-http"" />
        <uploadMethod ivo-id=""ivo://ivoa.net/std/TAPRegExt#upload-ftp"" />
        <retentionPeriod>
                <default>432000</default>
                <hard>604800</hard>
        </retentionPeriod>
        <executionDuration>
                <default>21600</default>
                <hard>86400</hard>
        </executionDuration>
        <outputLimit>
                <default  unit=""row"">1000000000</default>
                <hard  unit=""row"">1000000000</hard>
        </outputLimit>
        <uploadLimit>
                <default  unit=""row"">10000</default>
                <hard  unit=""row"">10000</hard>
        </uploadLimit>
        </capability>
        <capability  standardID=""ivo://ivoa.net/std/VOSI#capabilities"">
                <interface xsi:type=""vs:ParamHTTP"" role=""std"">
                        <accessURL use=""full""> http://localhost/TAPVizieR/tap/capabilities </accessURL>
                </interface>
        </capability>
        <capability  standardID=""ivo://ivoa.net/std/VOSI#availability"">
                <interface xsi:type=""vs:ParamHTTP"" role=""std"">
                        <accessURL use=""full""> http://localhost/TAPVizieR/tap/availability </accessURL>
                </interface>
        </capability>
        <capability  standardID=""ivo://ivoa.net/std/VOSI#tables"">
                <interface xsi:type=""vs:ParamHTTP"" role=""std"">
                        <accessURL use=""full""> http://localhost/TAPVizieR/tap/tables </accessURL>
                </interface>
        </capability>
</vosi:capabilities>";
        }
    }
}
