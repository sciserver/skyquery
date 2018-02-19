using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Jhu.SkyQuery.Tap.Client
{
    [TestClass]
    public class TapClientGaiaTest : TapClientTest
    {
        protected override Uri BaseUri
        {
            get
            {
                return new Uri(Constants.TapBaseUrlGaia);
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
            Assert.AreEqual("application/x-votable+xml;serialization=BINARY2", mime);
        }

        [TestMethod]
        public void SubmitQueryAsyncTest()
        {
            SubmitQueryAsyncTestHelper("SELECT TOP 10 ra, dec FROM gaiadr1.tgas_source", "application/x-votable+xml;serialization=BINARY2");
        }
    }
}
