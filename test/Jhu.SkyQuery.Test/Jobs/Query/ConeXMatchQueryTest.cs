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

namespace Jhu.SkyQuery.Jobs.Query.Test
{
    [TestClass]
    public class ConeXMatchQueryTest : SkyQueryTestBase
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
        [TestCategory("Query")]
        public void SimplestQueryTest()
        {
            var sql = @"
SELECT x.matchID, x.ra, x.dec, s.ra, s.dec, w.ra, w.dec
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS s,
    MUST EXIST IN TEST:WISEPhotoObj AS w,
    LIMIT CONE TO CIRCLE(s.RA, s.Dec, 2)
) AS x
";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void VariableRadiusTest()
        {
            var sql = @"
SELECT x.matchID, x.ra, x.dec, s.ra, s.dec, w.ra, w.dec
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS s,
    MUST EXIST IN TEST:WISEPhotoObj AS w,
    LIMIT CONE TO CIRCLE(w.RA, w.Dec, w.sigradec)
) AS x
WHERE w.sigradec > 0 AND w.sigradec < 9999
";

            RunQuery(sql);
        }
    }
}
