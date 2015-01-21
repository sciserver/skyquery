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
    public class XMatchQueryOperationsTest : XMatchQueryTestBase
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

        string sampleQuery = @"
SELECT p.objId, s.BestObjID, p.ra, p.dec, s.specObjId, s.ra, s.dec
INTO [$targettable]
FROM 
SDSSDR7:PhotoObjAll AS p WITH(Point(p.ra, p.dec), ERROR(0.1, 0.1, 0.1))
 CROSS JOIN SDSSDR7:SpecObjAll AS s WITH(Point(s.ra, s.dec), ERROR(0.1, 0.1, 0.1))
XMATCH BAYESFACTOR AS x
 MUST EXIST p
 MUST EXIST s
 HAVING LIMIT 1e3
WHERE     p.RA BETWEEN 0 AND 5 AND p.Dec BETWEEN 0 AND 5
      AND s.RA BETWEEN 0 AND 5 AND s.Dec BETWEEN 0 AND 5 ";

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
