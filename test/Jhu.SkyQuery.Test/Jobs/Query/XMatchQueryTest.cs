using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Test;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Scheduler;

namespace Jhu.SkyQuery.Jobs.Query.Test
{
    [TestClass]
    public class XMatchQueryTest : XMatchQueryTestBase
    {
        string sampleQuery = @"
SELECT p.objId, s.BestObjID, p.ra, p.dec, s.specObjId, s.ra, s.dec
INTO [$targettable]
FROM 
SDSSDR7:PhotoObjAll AS p
 CROSS JOIN SDSSDR7:SpecObjAll AS s
XMATCH BAYESFACTOR AS x
 MUST EXIST p ON Point(p.ra, p.dec), 0.1, 0.1, 0.1
 MUST EXIST s ON Point(s.ra, s.dec), 0.1, 0.1, 0.1
 HAVING LIMIT 1e3
WHERE     p.RA BETWEEN 0 AND 5 AND p.Dec BETWEEN 0 AND 5
      AND s.RA BETWEEN 0 AND 5 AND s.Dec BETWEEN 0 AND 5 ";

        [TestMethod]
        public void XMatchQuerySerializableTest()
        {
            var t = typeof(Jhu.SkyQuery.Jobs.Query.BayesFactorXMatchQuery);

            var sc = new Jhu.Graywulf.Activities.SerializableChecker();
            Assert.IsTrue(sc.Execute(t));
        }

        [TestMethod]
        public void RunXMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var guid = ScheduleQueryJob(
                    sampleQuery.Replace("[$targettable]", "XMatchQueryTest_RunXMatchQueryTest"),
                    QueueType.Long);

                WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                var ji = LoadJob(guid);
                Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
            }
        }

        [TestMethod]
        public void CancelXMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var guid = ScheduleQueryJob(
                    sampleQuery.Replace("[$targettable]", "XMatchQueryTest_CancelXMatchQueryTest"),
                    QueueType.Long);

                WaitJobStarted(guid, TimeSpan.FromSeconds(5));

                // *** TODO: it doesn't really want to cancel here but always completes
                CancelJob(guid);

                WaitJobComplete(guid, TimeSpan.FromSeconds(5));

                var ji = LoadJob(guid);
                Assert.IsTrue(ji.JobExecutionStatus == JobExecutionState.Cancelled ||
                    ji.JobExecutionStatus == JobExecutionState.Completed);
            }
        }

        [TestMethod]
        public void SuspendXMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var guid = ScheduleQueryJob(
                    sampleQuery.Replace("[$targettable]", "XMatchQueryTest_SuspendXMatchQueryTest"),
                    QueueType.Long);

                WaitJobStarted(guid, TimeSpan.FromSeconds(10));

                // Persist running jobs
                SchedulerTester.Instance.Stop();

                var ji = LoadJob(guid);
                Assert.IsTrue(ji.JobExecutionStatus == JobExecutionState.Persisted || ji.JobExecutionStatus == JobExecutionState.Completed);

                SchedulerTester.Instance.EnsureRunning();

                WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                ji = LoadJob(guid);
                Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
            }
        }

        [TestMethod]
        public void SelfJoinTest()
        {
            var sql =
@"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec
INTO [$targettable]
FROM SDSSDR7:PhotoObjAll AS s
CROSS JOIN SDSSDR7:PhotoObjAll AS g
XMATCH BAYESFACTOR x
MUST EXIST s ON POINT(s.cx, s.cy, s.cz), 0.1, 0.1, 0.1
MUST EXIST g ON POINT(g.cx, g.cy, g.cz), 0.2, 0.2, 0.2
HAVING LIMIT 1e3
WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5
	AND g.ra BETWEEN 0 AND 5 AND g.dec BETWEEN 0 AND 5";

            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();
                var guid = ScheduleQueryJob(
                                    sql.Replace("[$targettable]", "XMatchQueryTest_SelfJoinTest"),
                                    QueueType.Long);

                WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                var ji = LoadJob(guid);
                Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
            }
        }

        [TestMethod]
        public void CartesianCoordinatesTest()
        {
            var sql =
@"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec
INTO [$targettable]
FROM SDSSDR7:PhotoObjAll AS s
CROSS JOIN Galex:PhotoObjAll AS g
XMATCH BAYESFACTOR x
MUST EXIST s ON POINT(s.cx, s.cy, s.cz), 0.1, 0.1, 0.1
MUST EXIST g ON POINT(g.ra, g.dec), 0.2, 0.2, 0.2
HAVING LIMIT 1e3
WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5
	AND g.ra BETWEEN 0 AND 5 AND g.dec BETWEEN 0 AND 5";

            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();
                var guid = ScheduleQueryJob(
                                    sql.Replace("[$targettable]", "XMatchQueryTest_CartesianCoordinatesTest"),
                                    QueueType.Long);

                WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                var ji = LoadJob(guid);
                Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
            }
        }

        [TestMethod]
        public void ThreeWayJoinTest()
        {
            var sql =
@"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, t.objid, t.ra, t.dec, x.ra, x.dec
INTO [$targettable]
FROM SDSSDR7:PhotoObjAll AS s
CROSS JOIN Galex:PhotoObjAll AS g
CROSS JOIN TwoMASS:PhotoObj AS t
XMATCH BAYESFACTOR x
MUST EXIST s ON POINT(s.ra, s.dec), 0.1, 0.1, 0.1
MUST EXIST g ON POINT(g.ra, g.dec), 0.2, 0.2, 0.2
MUST EXIST t ON POINT(t.ra, t.dec), 0.5, 0.5, 0.5
HAVING LIMIT 1e3
WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5
	AND g.ra BETWEEN 0 AND 5 AND g.dec BETWEEN 0 AND 5
    AND t.ra BETWEEN 0 AND 5 AND t.dec BETWEEN 0 AND 5";

            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();
                var guid = ScheduleQueryJob(
                                    sql.Replace("[$targettable]", "XMatchQueryTest_ThreeWayJoinTest"),
                                    QueueType.Long);

                WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                var ji = LoadJob(guid);
                Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
            }
        }

        [TestMethod]
        public void ColumnOnlyInWhereClauseXMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var sql =
@"SELECT x.ra, x.dec
INTO [$targettable]
FROM SDSSDR7:PhotoObjAll AS s
CROSS JOIN GALEX:PhotoObjAll AS g
XMATCH BAYESFACTOR x
MUST EXIST s ON POINT(s.ra, s.dec), 0.1, 0.1, 0.1
MUST EXIST g ON POINT(g.ra, g.dec), 0.2, 0.2, 0.2
HAVING LIMIT 1e3
WHERE s.ra BETWEEN 0 AND 0.5 AND s.dec BETWEEN 0 AND 0.5 AND g.ra BETWEEN 0 AND 0.5 AND g.dec BETWEEN 0 AND 0.5
";

                var guid = ScheduleQueryJob(
                    sql.Replace("[$targettable]", "XMatchQueryTest_ColumnOnlyInWhereClauseXMatchQueryTest"),
                    QueueType.Long);

                WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                var ji = LoadJob(guid);
                Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
            }
        }

        [TestMethod]
        public void TinyRegionXMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var sql =
@"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec
INTO [$targettable]
FROM SDSSDR7:PhotoObjAll AS s
CROSS JOIN GALEX:PhotoObjAll AS g
XMATCH BAYESFACTOR x
MUST EXIST s ON POINT(s.ra, s.dec), 0.1, 0.1, 0.1
MUST EXIST g ON POINT(g.ra, g.dec), 0.2, 0.2, 0.2
HAVING LIMIT 1e3
WHERE s.ra BETWEEN 0 AND 0.5 AND s.dec BETWEEN 0 AND 0.5 AND g.ra BETWEEN 0 AND 0.5 AND g.dec BETWEEN 0 AND 0.5
";

                var guid = ScheduleQueryJob(
                    sql.Replace("[$targettable]", "XMatchQueryTest_TinyRegionXMatchQueryTest"),
                    QueueType.Long);

                WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                var ji = LoadJob(guid);
                Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
            }
        }

        [TestMethod]
        public void TinierRegionXMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var sql =
@"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec
INTO [$targettable]
FROM SDSSDR7:PhotoObjAll AS s
CROSS JOIN GALEX:PhotoObjAll AS g
XMATCH BAYESFACTOR x
MUST EXIST s ON POINT(s.ra, s.dec), 0.1, 0.1, 0.1
MUST EXIST g ON POINT(g.ra, g.dec), 0.2, 0.2, 0.2
HAVING LIMIT 1e3
WHERE s.ra BETWEEN 0 AND 0.01 AND s.dec BETWEEN 0 AND 0.01 AND g.ra BETWEEN 0 AND 0.01 AND g.dec BETWEEN 0 AND 0.01
";

                var guid = ScheduleQueryJob(
                    sql.Replace("[$targettable]", "XMatchQueryTest_TinierRegionXMatchQueryTest"),
                    QueueType.Long);

                WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                var ji = LoadJob(guid);
                Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
            }
        }

        [TestMethod]
        public void SelectStarXMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var sql =
@"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.*
INTO [$targettable]
FROM SDSSDR7:PhotoObjAll AS s
CROSS JOIN GALEX:PhotoObjAll AS g
XMATCH BAYESFACTOR x
MUST EXIST s ON POINT(s.ra, s.dec), 0.1, 0.1, 0.1
MUST EXIST g ON POINT(g.ra, g.dec), 0.2, 0.2, 0.2
HAVING LIMIT 1e3
WHERE s.ra BETWEEN 0 AND 0.5 AND s.dec BETWEEN 0 AND 0.5 AND g.ra BETWEEN 0 AND 0.5 AND g.dec BETWEEN 0 AND 0.5
";

                var guid = ScheduleQueryJob(
                    sql.Replace("[$targettable]", "XMatchQueryTest_SelectStarXMatchQueryTest"),
                    QueueType.Long);

                WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                var ji = LoadJob(guid);
                Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
            }
        }

        [TestMethod]
        public void SelectStarXMatchQueryTest2()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var sql =
@"SELECT s.objid, s.ra, s.dec, g.*, x.*
INTO [$targettable]
FROM SDSSDR7:PhotoObjAll AS s
CROSS JOIN GALEX:PhotoObjAll AS g
XMATCH BAYESFACTOR x
MUST EXIST s ON POINT(s.ra, s.dec), 0.1, 0.1, 0.1
MUST EXIST g ON POINT(g.ra, g.dec), 0.2, 0.2, 0.2
HAVING LIMIT 1e3
WHERE s.ra BETWEEN 0 AND 0.5 AND s.dec BETWEEN 0 AND 0.5 AND g.ra BETWEEN 0 AND 0.5 AND g.dec BETWEEN 0 AND 0.5
";

                var guid = ScheduleQueryJob(
                    sql.Replace("[$targettable]", "XMatchQueryTest_SelectStarXMatchQueryTest2"),
                    QueueType.Long);

                WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                var ji = LoadJob(guid);
                Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
            }
        }

        [TestMethod]
        public void NoErrorLimitsXMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var sql =
@"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec
INTO [$targettable]
FROM SDSSDR7:PhotoObjAll AS s
CROSS JOIN GALEX:PhotoObjAll AS g
XMATCH BAYESFACTOR x
MUST EXIST s ON POINT(s.ra, s.dec), 0.1
MUST EXIST g ON POINT(g.ra, g.dec), 0.2
HAVING LIMIT 1e3
WHERE s.ra BETWEEN 0 AND 0.5 AND s.dec BETWEEN 0 AND 0.5 AND g.ra BETWEEN 0 AND 0.5 AND g.dec BETWEEN 0 AND 0.5
";

                var guid = ScheduleQueryJob(
                    sql.Replace("[$targettable]", "XMatchQueryTest_TinyRegionXMatchQueryTest"),
                    QueueType.Long);

                WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                var ji = LoadJob(guid);
                Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
            }
        }

        [TestMethod]
        public void VariableErrorXMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var sql =
@"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec
INTO [$targettable]
FROM SDSSDR7:PhotoObjAll AS s
CROSS JOIN GALEX:PhotoObjAll AS g
XMATCH BAYESFACTOR x
MUST EXIST s ON POINT(s.ra, s.dec), s.raErr, 0.05, 0.1
MUST EXIST g ON POINT(g.ra, g.dec), 0.2
HAVING LIMIT 1e3
WHERE s.ra BETWEEN 0 AND 0.5 AND s.dec BETWEEN 0 AND 0.5 AND g.ra BETWEEN 0 AND 0.5 AND g.dec BETWEEN 0 AND 0.5
";

                var guid = ScheduleQueryJob(
                    sql.Replace("[$targettable]", "XMatchQueryTest_VariableErrorXMatchQueryTest"),
                    QueueType.Long);

                WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                var ji = LoadJob(guid);
                Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
            }
        }

        [TestMethod]
        public void JoinedTableMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var sql =
@"SELECT sp.specObjID, x.ra, x.dec
INTO [$targettable]
FROM SDSSDR7:PhotoObjAll AS s
INNER JOIN SDSSDR7:SpecObjAll sp ON sp.BestobjID = s.ObjID
CROSS JOIN GALEX:PhotoObjAll AS g
XMATCH BAYESFACTOR x
MUST EXIST s ON POINT(s.ra, s.dec), s.raErr, 0.05, 0.1
MUST EXIST g ON POINT(g.ra, g.dec), 0.2
HAVING LIMIT 1e3
WHERE s.ra BETWEEN 0 AND 0.5 AND s.dec BETWEEN 0 AND 0.5 AND g.ra BETWEEN 0 AND 0.5 AND g.dec BETWEEN 0 AND 0.5
";

                var guid = ScheduleQueryJob(
                    sql.Replace("[$targettable]", "XMatchQueryTest_JoinedTableMatchQueryTest"),
                    QueueType.Long);

                WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                var ji = LoadJob(guid);
                Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
            }
        }

        [TestMethod]
        public void MyDBMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var sql =
@"SELECT m.ra, m.dec, x.ra, x.dec
INTO [$targettable]
FROM SDSSDR7:PhotoObjAll AS s
CROSS JOIN MyCatalog m
XMATCH BAYESFACTOR x
MUST EXIST s ON POINT(s.ra, s.dec), s.raErr, 0.05, 0.1
MUST EXIST m ON POINT(m.ra, m.dec), 0.2
HAVING LIMIT 1e3
WHERE s.ra BETWEEN 0 AND 0.5 AND s.dec BETWEEN 0 AND 0.5
";

                var guid = ScheduleQueryJob(
                    sql.Replace("[$targettable]", "XMatchQueryTest_MyDBMatchQueryTest"),
                    QueueType.Long);

                WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                var ji = LoadJob(guid);
                Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
            }
        }

        [TestMethod]
        public void MyDBSelfXMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetExclusiveToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var sql =
@"SELECT m.ra, m.dec, x.ra, x.dec
INTO [$targettable]
FROM MyCatalog AS s
CROSS JOIN MyCatalog2 m
XMATCH BAYESFACTOR x
MUST EXIST s ON POINT(s.ra, s.dec), 0.3
MUST EXIST m ON POINT(m.ra, m.dec), 0.2
HAVING LIMIT 1e3
";

                var guid = ScheduleQueryJob(
                    sql.Replace("[$targettable]", "XMatchQueryTest_MyDBSelfXMatchQueryTest"),
                    QueueType.Long);

                WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                var ji = LoadJob(guid);
                Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
            }
        }
    }
}
