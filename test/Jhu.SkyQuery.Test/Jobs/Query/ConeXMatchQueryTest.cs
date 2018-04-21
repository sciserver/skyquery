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

        [TestMethod]
        [TestCategory("Query")]
        public void VariableRadiusWithRegionTest()
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
REGION 'CIRCLE J2000 0 0 10'
";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void VariableRadiusWithThreeCatalogsTest()
        {
            var sql = @"
SELECT x.matchID, x.ra, x.dec, s.ra, s.dec, w.ra, w.dec, g.ra, g.dec
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS s,
    MUST EXIST IN TEST:WISEPhotoObj AS w,
    MUST EXIST IN TEST:GalexPhotoObjAll AS g,
    LIMIT CONE TO CIRCLE(w.RA, w.Dec, 5 * w.sigradec)
) AS x
WHERE w.sigradec > 0 AND w.sigradec < 9999
";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void MyDbConstantRadiusTest()
        {
            var sql = @"
SELECT x.matchID, x.ra, x.dec, s.ra, s.dec, m.ra, m.dec
INTO [$into]
FROM XMATCH(
    MUST EXIST IN SDSSDR12:PhotoObjAll AS s,
    MUST EXIST IN MYDB:MyCatalog AS m WITH(POINT(ra, dec)),
    LIMIT CONE TO CIRCLE(m.RA, m.Dec, 2)
) AS x
";

            RunQuery(sql);
        }
    }
}
