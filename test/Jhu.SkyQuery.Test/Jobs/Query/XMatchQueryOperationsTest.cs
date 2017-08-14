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
    public class XMatchQueryOperationsTest : SkyQueryTestBase
    {

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            StartLogger();
            InitializeJobTests();
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            CleanupJobTests();
            StopLogger();
        }

        string sampleQuery = @"
SELECT p.objId, g.ObjID, p.ra, p.dec, g.ra, g.dec
INTO [$targettable]
FROM XMATCH
    (
        MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS p WITH(Point(p.ra, p.dec), ERROR(0.1, 0.1, 0.1)),
        MUST EXIST IN TEST:GalexPhotoObjAll AS g WITH(Point(g.ra, g.dec), ERROR(0.1, 0.1, 0.1)),
        LIMIT BAYESFACTOR TO 1e3) AS x
WHERE     p.RA BETWEEN 0 AND 5 AND p.Dec BETWEEN 0 AND 5
      AND g.RA BETWEEN 0 AND 5 AND g.Dec BETWEEN 0 AND 5 ";

        [TestMethod]
        [TestCategory("Query")]
        public void XMatchQuerySerializableTest()
        {
            var t = typeof(Jhu.SkyQuery.Jobs.Query.BayesFactorXMatchQuery);

            var sc = new Jhu.Graywulf.Activities.SerializableChecker();
            Assert.IsTrue(sc.Execute(t));
        }

        [TestMethod]
        [TestCategory("Query")]
        public void RunXMatchQueryTest()
        {
            using (SchedulerTester.Instance.GetToken())
            {
                SchedulerTester.Instance.EnsureRunning();

                var guid = ScheduleQueryJob(
                    sampleQuery.Replace("[$targettable]", "XMatchQueryTest_RunXMatchQueryTest"),
                    QueueType.Long);

                FinishQueryJob(guid);
            }
        }

        [TestMethod]
        [TestCategory("Query")]
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
        [TestCategory("Query")]
        public void SuspendXMatchQueryTest()
        {
            // TODO: this test can reproduce the NullException problems in the scheduler.
            // cause: QueryPartition collection is not populated after the job is resumed
            // from the persistence store.

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

    }
}
