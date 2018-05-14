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

namespace Jhu.SkyQuery.Sql.Jobs.Query
{
    [TestClass]
    public class SpecialXMatchQueryTest : SkyQueryTestBase
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

        protected override User SignInTestUser(RegistryContext context)
        {
            return SignInTestUser(context, "test.test", null);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void DotInUsernameXMatchTest()
        {
            var sql = @"
SELECT x.matchID, x.ra, x.dec
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll,
    MUST EXIST IN TEST:WISEPhotoObj,
    LIMIT BAYESFACTOR TO 1e3
) AS x
";

            RunQuery(sql);
        }
    }
}
