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
    public class XMatchQueryTest : SkyQueryTestBase
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
        public void SelfJoinTest()
        {
            var sql =
@"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN SDSSDR7:PhotoObjAll AS s WITH(POINT(s.cx, s.cy, s.cz), ERROR(0.1, 0.1, 0.1)),
     MUST EXIST IN SDSSDR7:PhotoObjAll AS g WITH(POINT(g.cx, g.cy, g.cz), ERROR(0.2, 0.2, 0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5
	AND g.ra BETWEEN 0 AND 5 AND g.dec BETWEEN 0 AND 5";

            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var guid = ScheduleQueryJob(
                                        sql.Replace("[$targettable]", "XMatchQueryTest_SelfJoinTest"),
                                        QueueType.Long);

                    FinishQueryJob(guid);
                }
            }
        }

        [TestMethod]
        [TestCategory("Query")]
        public void CartesianCoordinatesTest()
        {
            var sql =
@"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN SDSSDR7:PhotoObjAll AS s WITH(POINT(s.cx, s.cy, s.cz), ERROR(0.1, 0.1, 0.1)),
     MUST EXIST IN Galex:PhotoObjAll AS g WITH(POINT(g.ra, g.dec), ERROR(0.2, 0.2, 0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5
	AND g.ra BETWEEN 0 AND 5 AND g.dec BETWEEN 0 AND 5";

            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();
                var guid = ScheduleQueryJob(
                                    sql.Replace("[$targettable]", "XMatchQueryTest_CartesianCoordinatesTest"),
                                    QueueType.Long);

                FinishQueryJob(guid);
            }
        }

        [TestMethod]
        [TestCategory("Query")]
        public void ConstantErrorTest()
        {
            var sql =
@"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN SDSSDR7:PhotoObjAll AS s WITH(POINT(s.cx, s.cy, s.cz), ERROR(0.1)),
     MUST EXIST IN Galex:PhotoObjAll AS g WITH(POINT(g.ra, g.dec), ERROR(0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5
	AND g.ra BETWEEN 0 AND 5 AND g.dec BETWEEN 0 AND 5";

            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();
                var guid = ScheduleQueryJob(
                                    sql.Replace("[$targettable]", "XMatchQueryTest_ConstantErrorTest"),
                                    QueueType.Long);

                FinishQueryJob(guid);
            }
        }

        [TestMethod]
        [TestCategory("Query")]
        public void XMatchViewTest()
        {
            var sql =
@"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN SDSSDR7:PhotoObj AS s WITH(POINT(s.cx, s.cy, s.cz), ERROR(0.1, 0.1, 0.1)),
     MUST EXIST IN Galex:PhotoObjAll AS g WITH(POINT(g.ra, g.dec), ERROR(0.2, 0.2, 0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5
	AND g.ra BETWEEN 0 AND 5 AND g.dec BETWEEN 0 AND 5";

            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();
                var guid = ScheduleQueryJob(
                                    sql.Replace("[$targettable]", "XMatchQueryTest_XMatchViewTest"),
                                    QueueType.Long);

                FinishQueryJob(guid, new TimeSpan(0, 5, 0));
            }
        }

        [TestMethod]
        [TestCategory("Query")]
        public void XMatchViewOfViewTest()
        {
            var sql =
@"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN SDSSDR7:Star AS s WITH(POINT(s.cx, s.cy, s.cz), ERROR(0.1, 0.1, 0.1)),
     MUST EXIST IN Galex:PhotoObjAll AS g WITH(POINT(g.ra, g.dec), ERROR(0.2, 0.2, 0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5
	AND g.ra BETWEEN 0 AND 5 AND g.dec BETWEEN 0 AND 5";

            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();
                var guid = ScheduleQueryJob(
                                    sql.Replace("[$targettable]", "XMatchQueryTest_XMatchViewOfViewTest"),
                                    QueueType.Long);

                FinishQueryJob(guid, new TimeSpan(0, 5, 0));
            }
        }

        [TestMethod]
        [TestCategory("Query")]
        public void ThreeWayJoinTest()
        {
            var sql =
@"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, t.objid, t.ra, t.dec, x.ra, x.dec
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN SDSSDR7:PhotoObjAll AS s WITH(POINT(s.ra, s.dec), ERROR(0.1, 0.1, 0.1)),
     MUST EXIST IN Galex:PhotoObjAll AS g WITH(POINT(g.ra, g.dec), ERROR(0.2, 0.2, 0.2)),
     MUST EXIST IN TwoMASS:PhotoObj AS t WITH(POINT(t.ra, t.dec), ERROR(0.5, 0.5, 0.5)),
     LIMIT BAYESFACTOR TO 1e3) AS x
WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5
	AND g.ra BETWEEN 0 AND 5 AND g.dec BETWEEN 0 AND 5
    AND t.ra BETWEEN 0 AND 5 AND t.dec BETWEEN 0 AND 5";

            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();
                var guid = ScheduleQueryJob(
                                    sql.Replace("[$targettable]", "XMatchQueryTest_ThreeWayJoinTest"),
                                    QueueType.Long);

                FinishQueryJob(guid);
            }
        }

        [TestMethod]
        [TestCategory("Query")]
        public void ColumnOnlyInWhereClauseXMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var sql =
@"SELECT x.ra, x.dec
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN SDSSDR7:PhotoObjAll AS s WITH(POINT(s.ra, s.dec), ERROR(0.1, 0.1, 0.1)),
     MUST EXIST IN GALEX:PhotoObjAll AS g WITH(POINT(g.ra, g.dec), ERROR(0.2, 0.2, 0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
WHERE s.ra BETWEEN 0 AND 0.5 AND s.dec BETWEEN 0 AND 0.5 AND g.ra BETWEEN 0 AND 0.5 AND g.dec BETWEEN 0 AND 0.5
";

                var guid = ScheduleQueryJob(
                    sql.Replace("[$targettable]", "XMatchQueryTest_ColumnOnlyInWhereClauseXMatchQueryTest"),
                    QueueType.Long);

                FinishQueryJob(guid);
            }
        }

        [TestMethod]
        [TestCategory("Query")]
        public void TinyRegionXMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var sql =
@"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN SDSSDR7:PhotoObjAll AS s WITH(POINT(s.ra, s.dec), ERROR(0.1, 0.1, 0.1)),
     MUST EXIST IN GALEX:PhotoObjAll AS g WITH(POINT(g.ra, g.dec), ERROR(0.2, 0.2, 0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
WHERE s.ra BETWEEN 0 AND 0.5 AND s.dec BETWEEN 0 AND 0.5 AND g.ra BETWEEN 0 AND 0.5 AND g.dec BETWEEN 0 AND 0.5
";

                var guid = ScheduleQueryJob(
                    sql.Replace("[$targettable]", "XMatchQueryTest_TinyRegionXMatchQueryTest"),
                    QueueType.Long);

                FinishQueryJob(guid);
            }
        }

        [TestMethod]
        [TestCategory("Query")]
        public void TinierRegionXMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var sql =
@"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN SDSSDR7:PhotoObjAll AS s WITH(POINT(s.ra, s.dec), ERROR(0.1, 0.1, 0.1)),
     MUST EXIST IN GALEX:PhotoObjAll AS g WITH(POINT(g.ra, g.dec), ERROR(0.2, 0.2, 0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
WHERE s.ra BETWEEN 0 AND 0.01 AND s.dec BETWEEN 0 AND 0.01 AND g.ra BETWEEN 0 AND 0.01 AND g.dec BETWEEN 0 AND 0.01
";

                var guid = ScheduleQueryJob(
                    sql.Replace("[$targettable]", "XMatchQueryTest_TinierRegionXMatchQueryTest"),
                    QueueType.Long);

                FinishQueryJob(guid);
            }
        }

        [TestMethod]
        [TestCategory("Query")]
        public void SelectStarXMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var sql =
@"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.*
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN SDSSDR7:PhotoObjAll AS s WITH(POINT(s.ra, s.dec), ERROR(0.1, 0.1, 0.1)),
     MUST EXIST IN GALEX:PhotoObjAll AS g WITH(POINT(g.ra, g.dec), ERROR(0.2, 0.2, 0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
WHERE s.ra BETWEEN 0 AND 0.5 AND s.dec BETWEEN 0 AND 0.5 AND g.ra BETWEEN 0 AND 0.5 AND g.dec BETWEEN 0 AND 0.5
";

                var guid = ScheduleQueryJob(
                    sql.Replace("[$targettable]", "XMatchQueryTest_SelectStarXMatchQueryTest"),
                    QueueType.Long);

                FinishQueryJob(guid);
            }
        }

        [TestMethod]
        [TestCategory("Query")]
        public void SelectStarXMatchQueryTest2()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var sql =
@"SELECT s.objid, s.ra, s.dec, g.*, x.*
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN SDSSDR7:PhotoObjAll AS s WITH(POINT(s.ra, s.dec), ERROR(0.1, 0.1, 0.1)),
     MUST EXIST IN GALEX:PhotoObjAll AS g WITH(POINT(g.ra, g.dec), ERROR(0.2, 0.2, 0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
WHERE s.ra BETWEEN 0 AND 0.5 AND s.dec BETWEEN 0 AND 0.5 AND g.ra BETWEEN 0 AND 0.5 AND g.dec BETWEEN 0 AND 0.5
";

                var guid = ScheduleQueryJob(
                    sql.Replace("[$targettable]", "XMatchQueryTest_SelectStarXMatchQueryTest2"),
                    QueueType.Long);

                FinishQueryJob(guid);
            }
        }

        [TestMethod]
        [TestCategory("Query")]
        public void NoErrorLimitsXMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var sql =
@"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN SDSSDR7:PhotoObjAll AS s WITH(POINT(s.ra, s.dec), ERROR(0.1)),
     MUST EXIST IN GALEX:PhotoObjAll AS g WITH(POINT(g.ra, g.dec), ERROR(0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
WHERE s.ra BETWEEN 0 AND 0.5 AND s.dec BETWEEN 0 AND 0.5 AND g.ra BETWEEN 0 AND 0.5 AND g.dec BETWEEN 0 AND 0.5
";

                var guid = ScheduleQueryJob(
                    sql.Replace("[$targettable]", "XMatchQueryTest_TinyRegionXMatchQueryTest"),
                    QueueType.Long);

                FinishQueryJob(guid);
            }
        }

        [TestMethod]
        [TestCategory("Query")]
        public void VariableErrorXMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var sql =
@"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, x.dec, x.ra, x.dec
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN SDSSDR7:PhotoObjAll AS s WITH(POINT(s.ra, s.dec), ERROR(s.raErr, 0.05, 0.1)),
     MUST EXIST IN GALEX:PhotoObjAll AS g WITH(POINT(g.ra, g.dec), ERROR(0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
WHERE s.ra BETWEEN 0 AND 0.5 AND s.dec BETWEEN 0 AND 0.5 AND g.ra BETWEEN 0 AND 0.5 AND g.dec BETWEEN 0 AND 0.5
";

                var guid = ScheduleQueryJob(
                    sql.Replace("[$targettable]", "XMatchQueryTest_VariableErrorXMatchQueryTest"),
                    QueueType.Long);

                FinishQueryJob(guid);
            }
        }

        [TestMethod]
        [TestCategory("Query")]
        public void JoinedTableMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var sql =
@"SELECT sp.specObjID, x.ra, x.dec
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN SDSSDR7:PhotoObjAll AS s WITH(POINT(s.ra, s.dec), ERROR(s.raErr, 0.05, 0.1)),
     MUST EXIST IN GALEX:PhotoObjAll AS g WITH(POINT(g.ra, g.dec), ERROR(0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
    INNER JOIN SDSSDR7:SpecObjAll sp ON sp.BestobjID = s.ObjID
WHERE s.ra BETWEEN 0 AND 0.5 AND s.dec BETWEEN 0 AND 0.5 AND g.ra BETWEEN 0 AND 0.5 AND g.dec BETWEEN 0 AND 0.5
";

                var guid = ScheduleQueryJob(
                    sql.Replace("[$targettable]", "XMatchQueryTest_JoinedTableMatchQueryTest"),
                    QueueType.Long);

                FinishQueryJob(guid);
            }
        }

        [TestMethod]
        [TestCategory("Query")]
        public void MyDBXMatchSmallLimitsQueryTest()
        {
            // This test requires a table 'MyCatalog' in mydb

            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql =
    @"SELECT m.ra, m.dec, x.ra, x.dec
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN SDSSDR7:PhotoObjAll AS s WITH(POINT(s.ra, s.dec), ERROR(s.raErr, 0.05, 0.1)),
     MUST EXIST IN MyCatalog m WITH(POINT(m.ra, m.dec), ERROR(0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
WHERE s.ra BETWEEN 0 AND 0.5 AND s.dec BETWEEN 0 AND 0.5
";

                    var guid = ScheduleQueryJob(
                        sql.Replace("[$targettable]", "XMatchQueryTest_MyDBXMatchSmallLimitsQueryTest"),
                        QueueType.Long);

                    FinishQueryJob(guid);
                }
            }
        }


        [TestMethod]
        [TestCategory("Query")]
        public void MyDBXMatchLargeLimitsQueryTest()
        {
            // This test requires a table 'MyCatalog' in mydb

            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql =
    @"SELECT m.ra, m.dec, x.ra, x.dec
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN SDSSDR7:PhotoObjAll AS s WITH(POINT(s.ra, s.dec), ERROR(s.raErr, 0.05, 0.1)),
     MUST EXIST IN MyCatalog m WITH(POINT(m.ra, m.dec), ERROR(0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
WHERE s.ra BETWEEN 1 AND 3 AND s.dec BETWEEN 12 AND 14
";

                    var guid = ScheduleQueryJob(
                        sql.Replace("[$targettable]", "XMatchQueryTest_MyDBXMatchLargeLimitsQueryTest"),
                        QueueType.Long);

                    FinishQueryJob(guid);
                }
            }
        }

        [TestMethod]
        [TestCategory("Query")]
        public void MyDBXMatchNoLimitsQueryTest()
        {
            // This test requires a table 'MyCatalog' in mydb

            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql =
    @"SELECT m.ra, m.dec, x.ra, x.dec
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN SDSSDR7:PhotoObjAll AS s WITH(POINT(s.ra, s.dec), ERROR(s.raErr, 0.05, 0.1)),
     MUST EXIST IN MyCatalog m WITH(POINT(m.ra, m.dec), ERROR(0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
";

                    var guid = ScheduleQueryJob(
                        sql.Replace("[$targettable]", "XMatchQueryTest_MyDBXMatchNoLimitsQueryTest"),
                        QueueType.Long);

                    FinishQueryJob(guid, new TimeSpan(0, 30, 0));
                }
            }
        }

        [TestMethod]
        [TestCategory("Query")]
        public void MyDBSelfXMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql =
    @"SELECT m.ra, m.dec, x.ra, x.dec
INTO [$targettable]
FROM XMATCH
    (MUST EXIST IN MyCatalog AS s WITH(POINT(s.ra, s.dec), ERROR(0.3)),
     MUST EXIST IN MyCatalog2 m WITH(POINT(m.ra, m.dec), ERROR(0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x";

                    var guid = ScheduleQueryJob(
                        sql.Replace("[$targettable]", "XMatchQueryTest_MyDBSelfXMatchQueryTest"),
                        QueueType.Long);

                    FinishQueryJob(guid);

                }
            }
        }
    }
}
