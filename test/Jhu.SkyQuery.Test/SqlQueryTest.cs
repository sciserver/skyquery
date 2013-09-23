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

namespace Jhu.SkyQuery.Test
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
                DropMyDBTable("dbo", "SqlQueryTest_SimpleQueryTest");

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
                DropMyDBTable("dbo", "SqlQueryTest_JoinQueryTest");

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
                DropMyDBTable("dbo", "SqlQueryTest_JoinQueryTest2");

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
                DropMyDBTable("dbo", "SqlQueryTest_JoinQueryTest3");

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
                DropMyDBTable("dbo", "SqlQueryTest_SelfJoinQueryTest");

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
                DropMyDBTable("dbo", "SqlQueryTest_SimpleSelectStarQueryTest");

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
                DropMyDBTable("dbo", "SqlQueryTest_AliasSelectStarQueryTest");

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
                DropMyDBTable("dbo", "SqlQueryTest_TableValuedFunctionTest");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql = "SELECT * INTO SqlQueryTest_TableValuedFunctionTest FROM dbo.fHtmCoverCircleEq(0, 0, 10) htm";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                    var ji = LoadJob(guid);
                    Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
                }
            }
        }
    }
}
