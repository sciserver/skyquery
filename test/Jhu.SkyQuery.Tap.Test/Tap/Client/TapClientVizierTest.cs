using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.SkyQuery.Tap.Client
{
    [TestClass]
    public class TapClientVizierTest : TapClientTest
    {
        protected override Uri BaseUri
        {
            get
            {
                return new Uri("http://tapvizier.u-strasbg.fr/TAPVizieR/tap/");
            }
        }

        [TestMethod]
        public void GetAvailabilityTest()
        {
            var avail = GetAvailabilityTestHelper();
        }

        [TestMethod]
        public void GetCapabilitiesTest()
        {
            var cap = GetCapabilitiesTestHelper();
        }

        [TestMethod]
        public void GetBestFormatMimeType()
        {
            var mime = GetBestFormatMimeTypeTestHelper();
            Assert.AreEqual("text/xml", mime);
        }

        [TestMethod]
        public async Task SubmitQueryAsyncTest()
        {
            await SubmitQueryAsyncTestHelper("SELECT TOP 10 ra, dec FROM \"I/337/gaia\"", "text/xml");
        }

        [TestMethod]
        public async Task SubmitQuerySyncTest()
        {
            await SubmitQuerySyncTestHelper("SELECT TOP 10 ra, dec FROM \"I/337/gaia\"", "text/xml");
        }
    }
}
