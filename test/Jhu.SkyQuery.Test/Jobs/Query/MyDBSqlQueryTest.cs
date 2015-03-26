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
    public class MyDBSqlQueryTest : XMatchQueryTestBase
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

        /// <summary>
        /// Executes a simple query on a single MyDB table
        /// </summary>
        [TestMethod]
        [TestCategory("Query")]
        public void MyDBTableQueryTest()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropUserDatabaseTable(Jhu.Graywulf.Schema.SqlServer.Constants.DefaultSchemaName, "MyDBSqlQueryTest_MyDBTableQueryTest");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql = "SELECT TOP 10 objid INTO MyDBSqlQueryTest_MyDBTableQueryTest FROM MYDB:MyCatalog";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    FinishQueryJob(guid);
                }
            }
        }

        /// <summary>
        /// Joins two MyDB tables
        /// </summary>
        [TestMethod]
        [TestCategory("Query")]
        public void MyDBTableJoinedQueryTest1()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropUserDatabaseTable(Jhu.Graywulf.Schema.SqlServer.Constants.DefaultSchemaName, "MyDBSqlQueryTest_MyDBTableJoinedQueryTest1");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql =
        @"SELECT a.objid
INTO MyDBSqlQueryTest_MyDBTableJoinedQueryTest1
FROM MYDB:MyCatalog a
INNER JOIN MYDB:MySDSSSample b ON a.ObjID = b.objID";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    FinishQueryJob(guid);
                }
            }
        }

        /// <summary>
        /// Joins a MyDB table with a mirrored catalog table
        /// </summary>
        [TestMethod]
        [TestCategory("Query")]
        public void MyDBTableJoinedQueryTest2()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropUserDatabaseTable(Jhu.Graywulf.Schema.SqlServer.Constants.DefaultSchemaName, "MyDBSqlQueryTest_MyDBTableJoinedQueryTest2");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql =
        @"SELECT a.objid
INTO MyDBSqlQueryTest_MyDBTableJoinedQueryTest2
FROM SDSSDR7:PhotoObjAll a
INNER JOIN MYDB:MySDSSSample b ON a.ObjID = b.ObjID";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    FinishQueryJob(guid);
                }
            }
        }

        /// <summary>
        /// Executes a self-join on a MyDB table
        /// </summary>
        [TestMethod]
        [TestCategory("Query")]
        public void SelfJoinQueryTest()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropUserDatabaseTable(Jhu.Graywulf.Schema.SqlServer.Constants.DefaultSchemaName, "MyDBSqlQueryTest_SelfJoinQueryTest");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql = @"
SELECT TOP 100 a.objID, b.ObjID
INTO MyDBSqlQueryTest_SelfJoinQueryTest
FROM MyCatalog a
CROSS JOIN MyCatalog b";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    FinishQueryJob(guid);
                }
            }
        }

        /// <summary>
        /// Executes a self-join on a MyDB table while also joining in a
        /// catalog table. This one is interesting because it can test name
        /// collision issues of cached remote tables.
        /// </summary>
        [TestMethod]
        [TestCategory("Query")]
        public void SelfJoinQueryTest2()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                DropUserDatabaseTable(Jhu.Graywulf.Schema.SqlServer.Constants.DefaultSchemaName, "MyDBSqlQueryTest_SelfJoinQueryTest2");

                SchedulerTester.Instance.EnsureRunning();
                using (RemoteServiceTester.Instance.GetToken())
                {
                    RemoteServiceTester.Instance.EnsureRunning();

                    var sql = @"
SELECT TOP 100 p.ObjID, a.objID, b.ObjID
INTO MyDBSqlQueryTest_SelfJoinQueryTest2
FROM SDSSDR7:PhotoObj p
CROSS JOIN MyCatalog a
CROSS JOIN MyCatalog b";

                    var guid = ScheduleQueryJob(sql, QueueType.Long);

                    FinishQueryJob(guid);
                }
            }
        }
    }
}
