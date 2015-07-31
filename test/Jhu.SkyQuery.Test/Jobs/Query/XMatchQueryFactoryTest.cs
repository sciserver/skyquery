using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Scheduler;
using Jhu.Graywulf.RemoteService;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Jobs.Query;
using Jhu.SkyQuery.Jobs.Query;

namespace Jhu.SkyQuery.Jobs.Query.Test
{
    [TestClass]
    public class XMatchQueryFactoryTest : XMatchQueryTestBase
    {
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                PurgeTestJobs();
            }
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                if (SchedulerTester.Instance.IsRunning)
                {
                    SchedulerTester.Instance.DrainStop();
                }

                PurgeTestJobs();
            }
        }

        private QueryBase CreateQuery(string query)
        {
            var f = new SingleServerXMatchQueryFactory();
            return f.CreateQuery(query);
        }

        [TestMethod]
        public void TestSimpleQuery()
        {
            var q = CreateQuery("SELECT TOP 10 * FROM CatalogA");

            Assert.IsTrue(q is SqlQuery);
            Assert.IsFalse(q is XMatchQuery);
            Assert.IsFalse(q is BayesFactorXMatchQuery);
        }

        [TestMethod]
        public void TestXMatchQuery()
        {
            var q = CreateQuery(@"
SELECT * FROM
CatalogA a WITH(POINT(a.Ra, a.Dec)),
CatalogB b WITH(POINT(b.Ra, b.Dec))
XMATCH BAYESFACTOR AS x
MUST EXIST a
MUST EXIST b
HAVING LIMIT 1e3");

            Assert.IsTrue(q is SqlQuery);
            Assert.IsTrue(q is XMatchQuery);
            Assert.IsTrue(q is BayesFactorXMatchQuery);
        }
    }
}
