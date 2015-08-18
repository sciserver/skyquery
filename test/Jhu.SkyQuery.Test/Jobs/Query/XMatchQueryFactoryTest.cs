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
    public class XMatchQueryFactoryTest : SkyQueryTestBase
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

        private SqlQuery CreateQuery(string query)
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
    XMATCH
    (MUST EXIST IN CatalogA a WITH(POINT(a.Ra, a.Dec)),
     MUST EXIST IN CatalogB b WITH(POINT(b.Ra, b.Dec)),
     LIMIT BAYESFACTOR TO 1e3) AS x");

            Assert.IsTrue(q is SqlQuery);
            Assert.IsTrue(q is XMatchQuery);
            Assert.IsTrue(q is BayesFactorXMatchQuery);
        }
    }
}
