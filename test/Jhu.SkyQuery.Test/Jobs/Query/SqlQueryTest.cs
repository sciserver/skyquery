using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Registry;

namespace Jhu.SkyQuery.Jobs.Query.Test
{
    [TestClass]
    public class SqlQueryTest : SkyQueryTestBase
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

        [TestMethod]
        public void TimeOutLongQueryTest()
        {
            var sql = @"SELECT * INTO [$into] FROM SDSSDR12:Field";

            RunQuery(sql, QueueType.Quick, JobExecutionState.TimedOut, 1, new TimeSpan(0, 5, 0), false);
        }
    }
}
