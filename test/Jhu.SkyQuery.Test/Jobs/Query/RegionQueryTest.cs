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
    public class RegionQueryTest : SkyQueryTestBase
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
        public void SingleTableWithRegioAndWhereTest()
        {
            var sql =
@"SELECT s.objid
INTO [$targettable]
FROM TEST:SDSSDR7PhotoObjAll_NoZone AS s WITH(POINT(ra, dec), ERROR(0.1, 0.1, 0.1))
REGION 'CIRCLE J2000 0 0 60'
WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void JoinedTableWithRegionQueryTest()
        {
            var sql =
@"SELECT s.objid, g.objid
INTO [$targettable]
FROM TEST:SDSSDR7PhotoObjAll_NoZone AS s WITH(POINT(ra, dec), ERROR(0.1, 0.1, 0.1))
INNER JOIN TEST:SDSSDR7PhotoObjAll_NoZone AS g WITH(POINT(ra, dec), ERROR(0.2, 0.2, 0.2))
    ON s.objID = g.objID
REGION 'CIRCLE J2000 0 0 60'
WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5
    AND g.ra BETWEEN 0 AND 5 AND g.dec BETWEEN 0 AND 5";

            RunQuery(sql);
        }
    }
}
