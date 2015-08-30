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
    public class BayesFactorXMatchQueryTest : SkyQueryTestBase
    {
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            InitializeQueryTests();
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            CleanupQueryTests();
        }

        [TestMethod]
        [TestCategory("Query")]
        public void SimpleXMatchTest()
        {
            var sql = @"
SELECT x.matchID,
       a.objID, a.ra, a.dec, a.g, a.r, a.i, 
       b.cntr, b.ra, b.dec, b.w1mpro, b.w2mpro, b.w3mpro
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1, 0.1, 0.1), ZONEID(zoneID)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1, 0.1, 0.1), ZONEID(zoneID)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
WHERE a.ra BETWEEN 0 AND 1 AND
      b.ra BETWEEN 0 AND 1";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void XMatchWithHtmAndSmallRegionTest()
        {
            var sql = @"
SELECT x.matchID,
       a.objID, a.ra, a.dec, a.g, a.r, a.i, 
       b.cntr, b.ra, b.dec, b.w1mpro, b.w2mpro, b.w3mpro
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1, 0.1, 0.1), HTMID(htmid), ZONEID(zoneID)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1, 0.1, 0.1), HTMID(htmid), ZONEID(zoneID)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
REGION 'CIRCLE J2000 0 0 10'
WHERE a.ra BETWEEN 0 AND 1 AND
      b.ra BETWEEN 0 AND 1";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void XMatchWithHtmAndLargeRegionTest()
        {
            var sql = @"
SELECT x.matchID,
       a.objID, a.ra, a.dec, a.g, a.r, a.i, 
       b.cntr, b.ra, b.dec, b.w1mpro, b.w2mpro, b.w3mpro
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1, 0.1, 0.1), HTMID(htmid), ZONEID(zoneID)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1, 0.1, 0.1), HTMID(htmid), ZONEID(zoneID)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
REGION 'CIRCLE J2000 0 0 300'
WHERE a.ra BETWEEN 0 AND 1 AND
      b.ra BETWEEN 0 AND 1";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void XMatchWithRegionWithHtmNoZoneTest()
        {
            var sql = @"
SELECT x.matchID,
       a.objID, a.ra, a.dec, a.g, a.r, a.i, 
       b.cntr, b.ra, b.dec, b.w1mpro, b.w2mpro, b.w3mpro
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll_NoZone AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1, 0.1, 0.1), HTMID(htmid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1, 0.1, 0.1), HTMID(htmid)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
REGION 'CIRCLE J2000 0 0 10'
WHERE a.ra BETWEEN 0 AND 1 AND
      b.ra BETWEEN 0 AND 1";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void SelfJoinTest()
        {
            var sql =
@"SELECT x.matchID,
       a.objID, a.ra, a.dec, a.g, a.r, a.i, 
       b.objID, b.ra, b.dec, b.g, b.r, b.i,
       x.ra, x.dec
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1, 0.1, 0.1), ZONEID(zoneID)),
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1, 0.1, 0.1), ZONEID(zoneID)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
WHERE a.ra BETWEEN 0 AND 1 AND
      b.ra BETWEEN 0 AND 1";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void CartesianCoordinatesTest()
        {
            var sql =
@"SELECT x.matchID,
       a.objID, a.ra, a.dec, a.g, a.r, a.i, 
       b.cntr, b.ra, b.dec, b.w1mpro, b.w2mpro, b.w3mpro
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(cx, cy, cz), ERROR(0.1, 0.1, 0.1), HTMID(htmid), ZONEID(zoneID)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(cx, cy, cz), ERROR(0.1, 0.1, 0.1), HTMID(htmid), ZONEID(zoneID)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
REGION 'CIRCLE J2000 0 0 10'
WHERE a.ra BETWEEN 0 AND 1 AND
      b.ra BETWEEN 0 AND 1";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void EquatiorialCoordinatesTest()
        {
            var sql =
@"SELECT x.matchID,
       a.objID, a.ra, a.dec, a.g, a.r, a.i, 
       b.cntr, b.ra, b.dec, b.w1mpro, b.w2mpro, b.w3mpro
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec), ERROR(0.1, 0.1, 0.1), HTMID(htmid), ZONEID(zoneID)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec), ERROR(0.1, 0.1, 0.1), HTMID(htmid), ZONEID(zoneID)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
REGION 'CIRCLE J2000 0 0 10'
WHERE a.ra BETWEEN 0 AND 1 AND
      b.ra BETWEEN 0 AND 1";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void ConstantErrorTest()
        {
            // User specified error limits are only considered when more than two catalogs
            // are matched, so test this case carefully

            var sql =
@"SELECT x.matchID,
       a.objID, a.ra, a.dec, a.g, a.r, a.i, 
       b.cntr, b.ra, b.dec, b.w1mpro, b.w2mpro, b.w3mpro,
       c.objid, c.ra, c.dec, c.nuv_mag, c.fuv_mag
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid), ZONEID(zoneid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid), ZONEID(zoneid)),
    MUST EXIST IN TEST:GalexPhotoObjAll AS c WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.2), HTMID(htmid), ZONEID(zoneid)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
REGION 'CIRCLE J2000 0 0 10'";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void VariableErrorTest()
        {
            var sql =
@"SELECT x.matchID,
       a.objID, a.ra, a.dec, a.g, a.r, a.i, 
       b.cntr, b.ra, b.dec, b.w1mpro, b.w2mpro, b.w3mpro,
       c.objid, c.ra, c.dec, c.nuv_mag, c.fuv_mag
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(raErr + decErr, 0.1, 0.5), HTMID(htmid), ZONEID(zoneid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(sigRaDec, 0.1, 0.2), HTMID(htmid), ZONEID(zoneid)),
MUST EXIST IN TEST:GalexPhotoObjAll AS c WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.2), HTMID(htmid), ZONEID(zoneid)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
REGION 'CIRCLE J2000 0 0 10'
";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void VariableErrorWithNoLimitsTest()
        {
            var sql =
@"SELECT x.matchID,
       a.objID, a.ra, a.dec, a.g, a.r, a.i, 
       b.cntr, b.ra, b.dec, b.w1mpro, b.w2mpro, b.w3mpro
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(raErr + decErr), HTMID(htmid), ZONEID(zoneid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(sigRaDec), HTMID(htmid), ZONEID(zoneid)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
REGION 'CIRCLE J2000 0 0 10'
";

            RunQuery(sql);

            // TODO: this must fail with a meaningful error message
        }

        [TestMethod]
        [TestCategory("Query")]
        public void XMatchViewTest()
        {
            var sql =
@"SELECT x.matchID,
       a.objID, a.ra, a.dec, a.g, a.r, a.i, 
       b.cntr, b.ra, b.dec, b.w1mpro, b.w2mpro, b.w3mpro
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoPrimary AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid), ZONEID(zoneid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid), ZONEID(zoneid)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
REGION 'CIRCLE J2000 0 0 10'";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void XMatchViewOfViewTest()
        {
            // SDSSDR7Star is a view of view

            var sql =
@"SELECT x.matchID,
       a.objID, a.ra, a.dec, a.g, a.r, a.i, 
       b.cntr, b.ra, b.dec, b.w1mpro, b.w2mpro, b.w3mpro
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7Star AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid), ZONEID(zoneid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid), ZONEID(zoneid)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
REGION 'CIRCLE J2000 0 0 10'";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void ThreeWayJoinTest()
        {
            var sql =
@"SELECT x.matchID,
       a.objID, a.ra, a.dec, a.g, a.r, a.i, 
       b.cntr, b.ra, b.dec, b.w1mpro, b.w2mpro, b.w3mpro,
       c.objid, c.ra, c.dec, c.nuv_mag, c.fuv_mag
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid), ZONEID(zoneid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid), ZONEID(zoneid)),
    MUST EXIST IN TEST:GalexPhotoObjAll AS c WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.2), HTMID(htmid), ZONEID(zoneid)),
    LIMIT BAYESFACTOR TO 1e3
) AS x";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void ThreeWayJoinWithRegionTest()
        {
            // This test creates zone tables on the fly based on the search region
            // It can be used to test correct partitioning

            var sql =
@"SELECT x.matchID,
       a.objID, a.ra, a.dec, a.g, a.r, a.i, 
       b.cntr, b.ra, b.dec, b.w1mpro, b.w2mpro, b.w3mpro,
       c.objid, c.ra, c.dec, c.nuv_mag, c.fuv_mag
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid), ZONEID(zoneid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid), ZONEID(zoneid)),
    MUST EXIST IN TEST:GalexPhotoObjAll AS c WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.2), HTMID(htmid), ZONEID(zoneid)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
REGION 'CIRCLE J2000 0 0 10'";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void TableOutsideXMatchWithRegionTest()
        {
            // The region constraint only applies to the xmatch tables
            // but the query still should execute.

            var sql =
@"SELECT x.matchID,
       a.objID, a.ra, a.dec, a.g, a.r, a.i, 
       b.cntr, b.ra, b.dec, b.w1mpro, b.w2mpro, b.w3mpro,
       c.objid, c.ra, c.dec, c.nuv_mag, c.fuv_mag
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid), ZONEID(zoneid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid), ZONEID(zoneid)),
    LIMIT BAYESFACTOR TO 1e3
) AS x 
INNER JOIN TEST:GalexPhotoObjAll AS c WITH(POINT(ra, dec, cx, cy, cz), HTMID(htmid))
    ON c.ra = x.ra
REGION 'CIRCLE J2000 0 0 10'";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void ColumnOnlyInWhereClauseXMatchQueryTest()
        {
            // This is a query that requires building a zone table. At the same time, it
            // contains columns in the where clause that need to be propagated all
            // the way down to the final match table, so the where clause can be
            // applied at the end

            // It also tests the correct propagation of primary keys when not explicitly
            // referenced from the query

            var sql =
@"SELECT x.matchID, x.ra, x.dec
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid)),
    MUST EXIST IN TEST:GalexPhotoObjAll AS c WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.2), HTMID(htmid)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
REGION 'CIRCLE J2000 0 0 10'
WHERE a.g < b.w1mpro AND b.w2mpro < c.nuv_mag AND a.i < c.fuv_mag";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void TinyRegionXMatchQueryTest()
        {
            // Test correct partitioning of very small regions

            var sql =
@"SELECT x.matchID, x.ra, x.dec
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid)),
    MUST EXIST IN TEST:GalexPhotoObjAll AS c WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.2), HTMID(htmid)),
    LIMIT BAYESFACTOR TO 1e3) AS x
WHERE a.ra BETWEEN 0 AND 0.1 AND b.dec BETWEEN 0 AND 0.1 AND c.ra BETWEEN 0 AND 0.1 AND c.dec BETWEEN 0 AND 0.1
";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void TinierRegionXMatchQueryTest()
        {
            var sql =
@"SELECT x.matchID, x.ra, x.dec
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid)),
    MUST EXIST IN TEST:GalexPhotoObjAll AS c WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.2), HTMID(htmid)),
    LIMIT BAYESFACTOR TO 1e3) AS x
WHERE a.ra BETWEEN 0 AND 0.01 AND b.dec BETWEEN 0 AND 0.01 AND c.ra BETWEEN 0 AND 0.01 AND c.dec BETWEEN 0 AND 0.01
";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void SelectStarXMatchQueryTest()
        {
            // Select all columns from the computed table

            var sql =
@"SELECT x.*, a.objid
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid)),
    MUST EXIST IN TEST:GalexPhotoObjAll AS c WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.2), HTMID(htmid)),
    LIMIT BAYESFACTOR TO 1e3) AS x
REGION 'CIRCLE J2000 0 0 10'
";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void SelectStarXMatchQueryTest2()
        {
            // Select all columns from the computed table and
            // the catalogs

            var sql =
@"SELECT x.*, a.*, b.*, c.*
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid)),
    MUST EXIST IN TEST:GalexPhotoObjAll AS c WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.2), HTMID(htmid)),
    LIMIT BAYESFACTOR TO 1e3) AS x
REGION 'CIRCLE J2000 0 0 10'
";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void SelectStarXMatchQueryTest3()
        {
            // Select everything

            var sql =
@"SELECT *
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid)),
    MUST EXIST IN TEST:GalexPhotoObjAll AS c WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.2), HTMID(htmid)),
    LIMIT BAYESFACTOR TO 1e3) AS x
REGION 'CIRCLE J2000 0 0 10'
";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void JoinedTableMatchQueryTest()
        {
            var sql =
@"SELECT x.MatchID, x.ra, x.dec, rr.objID
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid)),
    MUST EXIST IN TEST:GalexPhotoObjAll AS c WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.2), HTMID(htmid)),
    LIMIT BAYESFACTOR TO 1e3) AS x
    INNER JOIN TEST:SDSSDR7PhotoObjAll_NoHtm rr ON rr.objid = a.objid
REGION 'CIRCLE J2000 0 0 10'
";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void JoinedTableWithRegionMatchQueryTest()
        {
            var sql =
@"SELECT x.MatchID, x.ra, x.dec, rr.objID
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid)),
    MUST EXIST IN TEST:GalexPhotoObjAll AS c WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.2), HTMID(htmid)),
    LIMIT BAYESFACTOR TO 1e3) AS x
    INNER JOIN TEST:SDSSDR7PhotoObjAll_NoZone rr WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid))
        ON rr.objid = a.objid
REGION 'CIRCLE J2000 0 0 10'
";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void NoLimitsTest()
        {
            // Full catalog xmatch

            var sql =
@"SELECT x.MatchID, x.ra, x.dec
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid)),
    MUST EXIST IN TEST:GalexPhotoObjAll AS c WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.2), HTMID(htmid)),
    LIMIT BAYESFACTOR TO 1e3) AS x
";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void XMatchWithMyDBTest()
        {
            // Full catalog xmatch with mydb table

            var sql =
@"SELECT x.MatchID, x.ra, x.dec
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid), ZONEID(zoneid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1), HTMID(htmid), ZONEID(zoneid)),
    MUST EXIST IN TEST:GalexPhotoObjAll AS c WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.2), HTMID(htmid), ZONEID(zoneid)),
    MUST EXIST IN MYDB:MyCatalog AS m WITH(POINT(ra, dec), ERROR(0.1)),
    LIMIT BAYESFACTOR TO 1e3) AS x
";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void MyDBSelfXMatchQueryTest()
        {
            var sql =
@"SELECT m.ra, m.dec, x.ra, x.dec
INTO [$into]
FROM XMATCH
    (MUST EXIST IN MyCatalog AS s WITH(POINT(s.ra, s.dec), ERROR(0.3)),
     MUST EXIST IN MyCatalog2 m WITH(POINT(m.ra, m.dec), ERROR(0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void XMatchRegionQueryWithNoHtmWithWhere()
        {
            var sql =
@"SELECT s.objid, g.objid, x.ra, x.dec
INTO [$into]
FROM XMATCH
    (MUST EXIST IN TEST:SDSSDR7PhotoObjAll_NoZone AS s WITH(POINT(ra, dec), ERROR(0.1, 0.1, 0.1)),
     MUST EXIST IN TEST:SDSSDR7PhotoObjAll_NoZone AS g WITH(POINT(ra, dec), ERROR(0.2, 0.2, 0.2)),
     LIMIT BAYESFACTOR TO 1e3) AS x
REGION 'CIRCLE J2000 0 0 60'
WHERE s.ra BETWEEN 0 AND 1";

            RunQuery(sql);
        }
    }
}
