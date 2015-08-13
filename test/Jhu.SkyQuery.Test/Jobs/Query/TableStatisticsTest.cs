using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Test;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Scheduler;
using Jhu.Graywulf.RemoteService;

namespace Jhu.SkyQuery.Jobs.Query.Test
{
    [TestClass]
    public class TableStatisticsTest : XMatchQueryTestBase
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

        private void RunQuery(string sql, string tablename)
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var guid = ScheduleQueryJob(
                        sql.Replace("[$targettable]", tablename),
                        QueueType.Long);

                    FinishQueryJob(guid);
                }
            }
        }

        [TestMethod]
        [TestCategory("Query")]
        public void SingleTableNoRegionQueryTest()
        {
            var sql =
@"SELECT s.objid
INTO [$targettable]
FROM TEST:SDSSDR7PhotoObjAll_NoZone AS s
WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5";

            RunQuery(sql, GetTestUniqueName());
        }

        [TestMethod]
        [TestCategory("Query")]
        public void JoinedTableNoRegionQueryTest()
        {
            var sql =
@"SELECT s.objid, g.objid
INTO [$targettable]
FROM TEST:SDSSDR7PhotoObjAll_NoZone AS s
INNER JOIN TEST:SDSSDR7PhotoObjAll_NoZone AS g
    ON s.objID = g.objID
WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5
    AND g.ra BETWEEN 0 AND 5 AND g.dec BETWEEN 0 AND 5";

            RunQuery(sql, GetTestUniqueName());
        }

        [TestMethod]
        [TestCategory("Query")]
        public void SingleTableWithRegionQueryTest()
        {
            var sql =
@"SELECT s.objid
INTO [$targettable]
FROM TEST:SDSSDR7PhotoObjAll_NoZone AS s WITH(POINT(ra, dec), ERROR(0.1, 0.1, 0.1))
REGION 'CIRCLE J2000 0 0 60'
WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5";

            RunQuery(sql, GetTestUniqueName());
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

            RunQuery(sql, GetTestUniqueName());
        }


        [TestMethod]
        [TestCategory("Query")]
        public void XMatchWithoutZoneIDTest()
        {
            var sql =
@"SELECT s.objid, g.objid
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN TEST:SDSSDR7PhotoObjAll_NoZone AS s WITH(POINT(ra, dec), ERROR(0.1, 0.1, 0.1)),
     MUST EXIST IN TEST:SDSSDR7PhotoObjAll_NoZone AS g WITH(POINT(ra, dec), ERROR(0.2, 0.2, 0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5
	AND g.ra BETWEEN 0 AND 5 AND g.dec BETWEEN 0 AND 5";

            RunQuery(sql, GetTestUniqueName());
        }

        [TestMethod]
        [TestCategory("Query")]
        public void XMatchRegionQueryWithHtm()
        {
            var sql =
@"SELECT s.objid, g.objid
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS s WITH(POINT(ra, dec), HTMID(htmid), ZONEID(zoneID), ERROR(0.1, 0.1, 0.1)),
     MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS g WITH(POINT(ra, dec), HTMID(htmid), ZONEID(zoneID), ERROR(0.2, 0.2, 0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
REGION 'CIRCLE J2000 0 0 60'";

            RunQuery(sql, GetTestUniqueName());
        }

        [TestMethod]
        [TestCategory("Query")]
        public void XMatchRegionQueryWithHtmNoZone()
        {
            var sql =
@"SELECT s.objid, g.objid
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN TEST:SDSSDR7PhotoObjAll_NoZone AS s WITH(POINT(ra, dec), HTMID(htmid), ERROR(0.1, 0.1, 0.1)),
     MUST EXIST IN TEST:SDSSDR7PhotoObjAll_NoZone AS g WITH(POINT(ra, dec), HTMID(htmid), ERROR(0.2, 0.2, 0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
REGION 'CIRCLE J2000 0 0 60'";

            RunQuery(sql, GetTestUniqueName());
        }

        [TestMethod]
        [TestCategory("Query")]
        public void XMatchRegionQueryWithNoHtm()
        {
            var sql =
@"SELECT s.objid, g.objid
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN TEST:SDSSDR7PhotoObjAll_NoZone AS s WITH(POINT(ra, dec), ERROR(0.1, 0.1, 0.1)),
     MUST EXIST IN TEST:SDSSDR7PhotoObjAll_NoZone AS g WITH(POINT(ra, dec), ERROR(0.2, 0.2, 0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
REGION 'CIRCLE J2000 0 0 60'";

            RunQuery(sql, GetTestUniqueName());
        }

        [TestMethod]
        [TestCategory("Query")]
        public void XMatchRegionQueryWithNoHtmWithWhere()
        {
            var sql =
@"SELECT s.objid, g.objid
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN TEST:SDSSDR7PhotoObjAll_NoZone AS s WITH(POINT(ra, dec), ERROR(0.1, 0.1, 0.1)),
     MUST EXIST IN TEST:SDSSDR7PhotoObjAll_NoZone AS g WITH(POINT(ra, dec), ERROR(0.2, 0.2, 0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
REGION 'CIRCLE J2000 0 0 60'
WHERE s.ra < 0";

            RunQuery(sql, GetTestUniqueName());
        }
    }
}
