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
    public class TapClientTest
    {
        private static readonly Uri baseUri = new Uri("http://tapvizier.u-strasbg.fr/TAPVizieR/tap/");
        private const string testQuery = "SELECT TOP 10 ra, dec FROM \"I/337/gaia\"";

        [TestMethod]
        public void SubmitQueryAsyncTest()
        {
            using (var tap = new TapClient(baseUri))
            {
                var job = new TapJob()
                {
                    Query = testQuery,
                };

                tap.SubmitAsync(job).Wait();
                tap.PollAsync(job, new[] { TapJobPhase.Completed }).Wait();

                var stream = tap.GetResultsAsync(job).Result;
                var reader = new System.IO.StreamReader(stream);
                var data = reader.ReadToEnd();
            }
        }
    }
}
