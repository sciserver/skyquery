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
    public class PartitionedSqlQueryTest : XMatchQueryTestBase
    {
        [TestMethod]
        public void SimpleQueryTest()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropMyDBTable("dbo", "PartitionedSqlQueryTest_SimpleQueryTest");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql =
        @"SELECT TOP 100 a.objid, a.ra, a.dec
INTO PartitionedSqlQueryTest_SimpleQueryTest
FROM SDSSDR7:PhotoObjAll a PARTITION ON a.objid
";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                    var ji = LoadJob(guid);
                    Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
                }
            }
        }

        [TestMethod]
        public void MyDBJoinQueryTest()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropMyDBTable("dbo", "PartitionedSqlQueryTest_MyDBJoinQueryTest");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql =
        @"SELECT TOP 100 a.objid, a.ra, a.dec, b.ObjID
INTO PartitionedSqlQueryTest_MyDBJoinQueryTest
FROM SDSSDR7:PhotoObjAll a PARTITION ON a.objid
CROSS JOIN MyCatalog b
";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    WaitJobComplete(guid, TimeSpan.FromSeconds(10));

                    var ji = LoadJob(guid);
                    Assert.AreEqual(JobExecutionState.Completed, ji.JobExecutionStatus);
                }
            }
        }
    }
}
