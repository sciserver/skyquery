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
    public class SqlQueryTest : XMatchQueryTestBase
    {

        [TestMethod]
        public void SqlQuerySerializableTest()
        {
            var t = typeof(Jhu.Graywulf.Jobs.Query.SqlQuery);

            var sc = new Jhu.Graywulf.Activities.SerializableChecker();
            Assert.IsTrue(sc.Execute(t));
        }

        [TestMethod]
        public void SimpleQueryTest()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropUserDatabaseTable("dbo", "SqlQueryTest_SimpleQueryTest");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql = "SELECT TOP 10 objid, ra, dec INTO SqlQueryTest_SimpleQueryTest FROM SDSSDR7:PhotoObj";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                    var ji = LoadJob(guid);
                    Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
                }
            }
        }

        /// <summary>
        /// Joins two tables of the same dataset.
        /// This one won't create a primary key on the target table because there's no
        /// unique combination of columns.
        /// </summary>
        [TestMethod]
        public void JoinQueryTest()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropUserDatabaseTable("dbo", "SqlQueryTest_JoinQueryTest");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql = @"
SELECT TOP 10 p.objid, p.ra, p.dec, s.ra, s.dec
INTO SqlQueryTest_JoinQueryTest
FROM SDSSDR7:PhotoObj p
INNER JOIN SDSSDR7:SpecObjAll s
    ON p.objID = s.bestObjID";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                    var ji = LoadJob(guid);
                    Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
                }
            }
        }

        /// <summary>
        /// Joins two tables of the same dataset.
        /// This one does create a primary key on the target table.
        /// </summary>
        [TestMethod]
        public void JoinQueryTest2()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropUserDatabaseTable("dbo", "SqlQueryTest_JoinQueryTest2");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql = @"
SELECT TOP 10 p.objid, s.specObjID, p.ra, p.dec, s.ra, s.dec
INTO SqlQueryTest_JoinQueryTest2
FROM SDSSDR7:PhotoObj p
INNER JOIN SDSSDR7:SpecObjAll s
    ON p.objID = s.bestObjID";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                    var ji = LoadJob(guid);
                    Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
                }
            }
        }

        /// <summary>
        /// Joins two table from different mirrored datasets.
        /// </summary>
        [TestMethod]
        public void JoinQueryTest3()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropUserDatabaseTable("dbo", "SqlQueryTest_JoinQueryTest3");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql = @"
SELECT TOP 100 s.objID, g.ObjID
INTO SqlQueryTest_JoinQueryTest3
FROM SDSSDR7:PhotoObjAll s
CROSS JOIN Galex:PhotoObjAll g";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                    var ji = LoadJob(guid);
                    Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
                }
            }
        }

        /// <summary>
        /// Executes a self-join
        /// </summary>
        [TestMethod]
        public void SelfJoinQueryTest()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropUserDatabaseTable("dbo", "SqlQueryTest_SelfJoinQueryTest");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql = @"
SELECT TOP 100 a.objID, b.ObjID
INTO SqlQueryTest_SelfJoinQueryTest
FROM SDSSDR7:PhotoObjAll a
CROSS JOIN SDSSDR7:PhotoObjAll b";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                    var ji = LoadJob(guid);
                    Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
                }
            }
        }

        [TestMethod]
        public void SimpleSelectStarQueryTest()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropUserDatabaseTable("dbo", "SqlQueryTest_SimpleSelectStarQueryTest");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql = "SELECT TOP 10 * INTO SqlQueryTest_SimpleSelectStarQueryTest FROM SDSSDR7:PhotoObj";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                    var ji = LoadJob(guid);
                    Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
                }
            }
        }

        [TestMethod]
        public void AliasSelectStarQueryTest()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropUserDatabaseTable("dbo", "SqlQueryTest_AliasSelectStarQueryTest");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql = "SELECT TOP 10 p.* INTO SqlQueryTest_AliasSelectStarQueryTest FROM SDSSDR7:PhotoObj p";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                    var ji = LoadJob(guid);
                    Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
                }
            }
        }

        [TestMethod]
        public void TableValuedFunctionTest()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropUserDatabaseTable("dbo", "SqlQueryTest_TableValuedFunctionTest");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql = "SELECT * INTO SqlQueryTest_TableValuedFunctionTest FROM dbo.fHtmCoverCircleEq(0, 0, 10) AS htm";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                    var ji = LoadJob(guid);
                    Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
                }
            }
        }

        [TestMethod]
        public void TableValuedFunctionJoinTest()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropUserDatabaseTable("dbo", "SqlQueryTest_TableValuedFunctionTest");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql = @"
SELECT TOP 100 objid, ra, dec
INTO SqlQueryTest_TableValuedFunctionJoinTest
FROM dbo.fHtmCoverCircleEq(0, 0, 10) htm
INNER JOIN SDSSDR7:PhotoObj p
    ON p.htmid BETWEEN htm.htmidstart AND htm.htmidend";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                    var ji = LoadJob(guid);
                    Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
                }
            }
        }

        [TestMethod]
        public void TableValuedFunctionCrossApplyTest()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropUserDatabaseTable("dbo", "SqlQueryTest_TableValuedFunctionCrossApplyTest");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql = @"
SELECT htm.htmidstart, htm.htmidend
INTO SqlQueryTest_TableValuedFunctionCrossApplyTest
FROM (SELECT TOP 10 ra, dec FROM SDSSDR7:PhotoObj) p
CROSS APPLY dbo.fHtmCoverCircleEq(p.ra, p.dec, 10) htm";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                    var ji = LoadJob(guid);
                    Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
                }
            }
        }

        [TestMethod]
        public void ScalarFunctionTest()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropUserDatabaseTable("dbo", "SqlQueryTest_ScalarFunctionTest");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql = @"
SELECT dbo.fDistanceEq(0, 0, 1, 1)
INTO SqlQueryTest_ScalarFunctionTest";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                    var ji = LoadJob(guid);
                    Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
                }
            }
        }

        [TestMethod]
        public void ScalarFunctionOnTableTest()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropUserDatabaseTable("dbo", "SqlQueryTest_ScalarFunctionOnTableTest");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql = @"
SELECT TOP 100 dbo.fDistanceEq(0, 0, p.ra, p.dec) sep
INTO SqlQueryTest_ScalarFunctionOnTableTest
FROM SDSSDR7:PhotoObj p";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                    var ji = LoadJob(guid);
                    Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
                }
            }
        }

        [TestMethod]
        public void ScalarFunctionInWhereTest()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropUserDatabaseTable("dbo", "SqlQueryTest_ScalarFunctionInWhereTest");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql = @"
SELECT p.objid, p.ra, p.dec
INTO SqlQueryTest_ScalarFunctionInWhereTest
FROM (SELECT TOP 100 * FROM SDSSDR7:PhotoObj) p
WHERE dbo.fDistanceEq(0, 0, p.ra, p.dec) > 1000";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                    var ji = LoadJob(guid);
                    Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
                }
            }
        }
    }
}
