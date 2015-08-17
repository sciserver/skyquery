using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Scheduler;
using Jhu.Graywulf.RemoteService;
using Jhu.Graywulf.Registry;
using Jhu.SkyQuery.Jobs.Query;

namespace Jhu.SkyQuery.Jobs.Query.Test
{
    [TestClass]
    public class RegionQueryTest : XMatchQueryTestBase
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

        [TestMethod]
        [TestCategory("Query")]
        public void SimpleRegionQueryTest()
        {
            var sql = @"
SELECT TOP 10 objid, ra, dec INTO [$into]
FROM SDSSDR7:PhotoObj WITH (POINT(ra, dec), HTMID(htmid))
REGION 'CIRCLE J2000 20 30 10'";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void XMatchRegionQueryTest()
        {
            var sql = @"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec
INTO [$into]
FROM XMATCH
    (MUST EXIST IN SDSSDR7:PhotoObjAll AS s WITH(POINT(s.ra, s.dec, s.cx, s.cy, s.cz), HTMID(s.htmid), ERROR(0.1, 0.1, 0.1)),
     MUST EXIST IN Galex:PhotoObjAll AS g WITH(POINT(g.ra, g.dec, g.cx, g.cy, g.cz), HTMID(g.htmid), ERROR(0.2, 0.2, 0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
REGION 'CIRCLE J2000 2.5 2.5 120'";

            RunQuery(sql);
        }
    }
}
