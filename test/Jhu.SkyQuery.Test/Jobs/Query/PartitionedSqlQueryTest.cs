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
    public class PartitionedSqlQueryTest : XMatchQueryTestBase
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
        public void SimpleQueryTest()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropUserDatabaseTable("dbo", "PartitionedSqlQueryTest_SimpleQueryTest");

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

                    FinishQueryJob(guid, new TimeSpan(0, 5, 0));
                }
            }
        }

        [TestMethod]
        [TestCategory("Query")]
        public void MyDBJoinQueryTest()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropUserDatabaseTable("dbo", "PartitionedSqlQueryTest_MyDBJoinQueryTest");

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

                    FinishQueryJob(guid);
                }
            }
        }
    }
}
