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
        private const string testQuery = "SELECT TOP 10 ra, dec FROM \"I/337/gaia\"";

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
        public void SubmitQueryAsyncTest()
        {
            SubmitQueryAsyncTestHelper(testQuery);
        }
    }
}
