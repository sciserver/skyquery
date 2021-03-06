﻿using System;
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
    public class BayesFactorXMatchQueryTest : Jhu.SkyQuery.SkyQueryTestBase
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
        public void SimplestXMatchTest()
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
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a,
    MUST EXIST IN TEST:WISEPhotoObj AS b,
    LIMIT BAYESFACTOR TO 1e3
) AS x
WHERE a.ra BETWEEN 0 AND 1 AND
      b.ra BETWEEN 0 AND 1";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void XMatchWithoutMatchIDTest()
        {
            // The primary key is not part of the select list

            var sql = @"
SELECT b.cntr, b.ra, b.dec, b.w1mpro, b.w2mpro, b.w3mpro
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
WHERE a.ra BETWEEN 0 AND 1 AND
      b.ra BETWEEN 0 AND 1
REGION 'CIRCLE J2000 0 0 10'";

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
WHERE a.ra BETWEEN 0 AND 1 AND
      b.ra BETWEEN 0 AND 1
REGION 'CIRCLE J2000 0 0 300'";

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
WHERE a.ra BETWEEN 0 AND 1 AND
      b.ra BETWEEN 0 AND 1
REGION 'CIRCLE J2000 0 0 10'";

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
WHERE a.ra BETWEEN 0 AND 1 AND
      b.ra BETWEEN 0 AND 1
REGION 'CIRCLE J2000 0 0 10'";

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
WHERE a.ra BETWEEN 0 AND 1 AND
      b.ra BETWEEN 0 AND 1
REGION 'CIRCLE J2000 0 0 10'";

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
WHERE sigRaDec < 9999
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
WHERE sigRaDec < 9999
REGION 'CIRCLE J2000 0 0 10'
";

            RunQuery(sql);

            // TODO: this must fail with a meaningful error message
        }

        [TestMethod]
        [TestCategory("Query")]
        public void VariableErrorWithLargeRegionTest()
        {
            // TODO: this should throw an exception because search radius is too large
            // (due to the value of 9999 in sigRaDec) but it fails with an exception
            // being thrown from the cancel branch of Retry. This is an async issue.

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
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.2), HTMID(htmid), ZONEID(zoneid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.5), HTMID(htmid), ZONEID(zoneid)),
    MUST EXIST IN TEST:GalexPhotoObjAll AS c WITH(POINT(ra, dec, cx, cy, cz), ERROR(2.0), HTMID(htmid), ZONEID(zoneid)),
    LIMIT BAYESFACTOR TO 1e3
) AS x";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void ThreeWayJoinWithWhereTest()
        {
            var sql =
@"SELECT x.matchID,
       a.objID, a.ra, a.dec, a.g, a.r, a.i, 
       b.cntr, b.ra, b.dec, b.w1mpro, b.w2mpro, b.w3mpro,
       c.objid, c.ra, c.dec, c.nuv_mag, c.fuv_mag
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.2), HTMID(htmid), ZONEID(zoneid)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.5), HTMID(htmid), ZONEID(zoneid)),
    MUST EXIST IN TEST:GalexPhotoObjAll AS c WITH(POINT(ra, dec, cx, cy, cz), ERROR(2.0), HTMID(htmid), ZONEID(zoneid)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
WHERE a.RA BETWEEN 0 AND 5 AND a.dec BETWEEN 0 AND 5
	AND b.RA BETWEEN 0 AND 5 AND b.dec BETWEEN 0 AND 5
    AND c.RA BETWEEN 0 AND 5 AND c.dec BETWEEN 0 AND 5";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void ThreeWayJoinWithWhereNoZoneIDTest()
        {
            var sql =
@"SELECT x.matchID,
       a.objID, a.ra, a.dec, a.g, a.r, a.i, 
       b.cntr, b.ra, b.dec, b.w1mpro, b.w2mpro, b.w3mpro,
       c.objid, c.ra, c.dec, c.nuv_mag, c.fuv_mag
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.2)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.5)),
    MUST EXIST IN TEST:GalexPhotoObjAll AS c WITH(POINT(ra, dec, cx, cy, cz), ERROR(2.0)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
WHERE a.RA BETWEEN 0 AND 5 AND a.dec BETWEEN 0 AND 5
	AND b.RA BETWEEN 0 AND 5 AND b.dec BETWEEN 0 AND 5
    AND c.RA BETWEEN 0 AND 5 AND c.dec BETWEEN 0 AND 5";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void ThreeWayJoinWithWhereNoHintsTest()
        {
            var sql =
@"SELECT x.matchID,
       a.objID, a.ra, a.dec, a.g, a.r, a.i, 
       b.cntr, b.ra, b.dec, b.w1mpro, b.w2mpro, b.w3mpro,
       c.objid, c.ra, c.dec, c.nuv_mag, c.fuv_mag
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(ERROR(0.2)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(ERROR(0.5)),
    MUST EXIST IN TEST:GalexPhotoObjAll AS c WITH(ERROR(2.0)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
WHERE a.RA BETWEEN 0 AND 5 AND a.dec BETWEEN 0 AND 5
	AND b.RA BETWEEN 0 AND 5 AND b.dec BETWEEN 0 AND 5
    AND c.RA BETWEEN 0 AND 5 AND c.dec BETWEEN 0 AND 5";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void ThreeWayJoinWithWhereTest2()
        {
            // Work on real databases
            // This often throws null exception in cancellationcontext
            // could this be an async issue (dispose called before cancellation)

            var sql = @"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec, t.ra, t.dec
INTO [$into]
FROM XMATCH
     (MUST EXIST IN SDSSDR7:PhotoObjAll AS s WITH(POINT(s.ra, s.dec, s.cx, s.cy, s.cz), ERROR(0.2, 0.2, 0.2), HTMID(htmid), ZONEID(zoneid)),
      MUST EXIST IN GALEX:PhotoObjAll AS g WITH(POINT(g.ra, g.dec, g.cx, g.cy, g.cz), ERROR(2.0, 2.0, 2.0), HTMID(htmid), ZONEID(zoneid)),
      MUST EXIST IN TwoMass:PhotoPSC AS t WITH(POINT(t.ra, t.dec, t.cx, t.cy, t.cz), ERROR(0.4, 0.4, 0.4), HTMID(htmid), ZONEID(zoneid)),
      LIMIT BAYESFACTOR TO 1e3) AS x
WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5
	AND g.ra BETWEEN 0 AND 5 AND g.dec BETWEEN 0 AND 5
    AND t.ra BETWEEN 0 AND 5 AND t.dec BETWEEN 0 AND 5
";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void ThreeWayJoinWithWhereTest3()
        {
            var sql = @"SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec, t.ra, t.dec
INTO [$into]
FROM XMATCH
     (MUST EXIST IN SDSSDR7:PhotoObjAll AS s WITH(POINT(s.ra, s.dec, s.cx, s.cy, s.cz), ERROR(0.2), HTMID(htmid), ZONEID(zoneid)),
      MUST EXIST IN WISE:PhotoObj AS t WITH(POINT(t.ra, t.dec, t.cx, t.cy, t.cz), ERROR(0.5), HTMID(htmid), ZONEID(zoneid)),
      MUST EXIST IN GALEX:PhotoObjAll AS g WITH(POINT(g.ra, g.dec, g.cx, g.cy, g.cz), ERROR(2.0), HTMID(htmid), ZONEID(zoneid)),
      LIMIT BAYESFACTOR TO 1e3) AS x
WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5
	AND g.ra BETWEEN 0 AND 5 AND g.dec BETWEEN 0 AND 5
    AND t.ra BETWEEN 0 AND 5 AND t.dec BETWEEN 0 AND 5
";

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
WHERE a.g < b.w1mpro AND b.w2mpro < c.nuv_mag AND a.i < c.fuv_mag
REGION 'CIRCLE J2000 0 0 10'";

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
        public void XMatchWithMyDBTest2()
        {
            // Full catalog xmatch with mydb table

            var sql =
@"SELECT x.MatchID, x.ra, x.dec
INTO [$into]
FROM XMATCH(
    MUST EXIST IN MYDB:MyCatalog AS m WITH(POINT(ra, dec), ERROR(0.1)),    
    MUST EXIST IN SDSSDR12:PhotoObjAll AS s WITH(ERROR(0.1)),
    LIMIT BAYESFACTOR TO 1e3) AS x
";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void XMatchWithMyDBTest3()
        {
            // Full catalog xmatch with mydb table

            var sql =
@"SELECT x.MatchID, x.ra, x.dec
INTO [$into]
FROM XMATCH(
    MUST EXIST IN SDSSDR12:PhotoObjAll AS s WITH(ERROR(0.1)),    
    MUST EXIST IN MYDB:MyCatalog AS m WITH(POINT(ra, dec), ERROR(0.1)),    
    LIMIT BAYESFACTOR TO 1e3) AS x
";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void XMatchWithMyDBNoPrimaryKeyTest()
        {
            // Full catalog xmatch with mydb table

            var sql =
@"SELECT x.MatchID, x.ra, x.dec
INTO [$into]
FROM XMATCH(
    MUST EXIST IN SDSSDR12:PhotoObjAll AS s WITH(ERROR(0.1)),    
    MUST EXIST IN MYDB:MyCatalog_NoPK AS m WITH(POINT(ra, dec), ERROR(0.1)),    
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
WHERE s.ra BETWEEN 0 AND 1
REGION 'CIRCLE J2000 0 0 60'";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void CreatePrimaryKeyTest()
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
        public void XMatchWithXMatchOutputTest()
        {
            // this now creates a table without primary key
            // Can be used to test xmatch w/o PK behavior

            DropUserDatabaseTable("XMatchWithXMatchOutput1");

            var sql = @"
SELECT x.matchID AS matchID,
       x.ra AS ra, x.dec AS dec,
        x.cx AS cx, x.cy AS cy, x.cz AS cz
INTO XMatchWithXMatchOutput1
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1, 0.1, 0.1), ZONEID(zoneID)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1, 0.1, 0.1), ZONEID(zoneID)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
WHERE a.ra BETWEEN 0 AND 1 AND
      b.ra BETWEEN 0 AND 1";

            RunQuery(sql);

            sql = @"
SELECT x.matchID,
       a.objID, a.ra, a.dec, 
       b.ra, b.dec
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1, 0.1, 0.1), ZONEID(zoneID)),
    MUST EXIST IN MYDB:XMatchWithXMatchOutput1 AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1, 0.1, 0.1)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
WHERE a.ra BETWEEN 0 AND 1 AND
      b.ra BETWEEN 0 AND 1
";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void XMatchWithXMatchOutputTest2()
        {
            // this test creates a table with an automatic PK that has identity definition set
            // make sure the second query works which selects the match id

            DropUserDatabaseTable("XMatchWithXMatchOutput1");

            var sql = @"
SELECT x.ra AS ra, x.dec AS dec,
        x.cx AS cx, x.cy AS cy, x.cz AS cz
INTO XMatchWithXMatchOutput1
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1, 0.1, 0.1), ZONEID(zoneID)),
    MUST EXIST IN TEST:WISEPhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1, 0.1, 0.1), ZONEID(zoneID)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
WHERE a.ra BETWEEN 0 AND 1 AND
      b.ra BETWEEN 0 AND 1";

            RunQuery(sql);

            sql = @"
SELECT b.__ID,
       a.objID, a.ra, a.dec, 
       b.ra, b.dec
INTO [$into]
FROM XMATCH(
    MUST EXIST IN TEST:SDSSDR7PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1, 0.1, 0.1), ZONEID(zoneID)),
    MUST EXIST IN MYDB:XMatchWithXMatchOutput1 AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1, 0.1, 0.1)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
WHERE a.ra BETWEEN 0 AND 1 AND
      b.ra BETWEEN 0 AND 1
";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void TimeOutXMatchQueryTest()
        {
            var sql = @"
SELECT x.matchID
INTO [$into]
FROM XMATCH(
    MUST EXIST IN SDSSDR12:PhotoObjAll AS a WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1, 0.1, 0.1), ZONEID(zoneID)),
    MUST EXIST IN WISE:PhotoObj AS b WITH(POINT(ra, dec, cx, cy, cz), ERROR(0.1, 0.1, 0.1), ZONEID(zoneID)),
    LIMIT BAYESFACTOR TO 1e3
) AS x
";

            RunQuery(sql, QueueType.Quick, JobExecutionState.TimedOut, 0, new TimeSpan(0, 5, 0), false);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void BayesFactorTest()
        {
            var sql = @"
SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.* 
INTO [$into] 
FROM XMATCH
    (MUST EXIST IN MyDB:BayesFactorTest AS s WITH(POINT(s.ra, s.dec), ERROR(1)),
     MUST EXIST IN MyDB:BayesFactorTest AS g WITH(POINT(g.ra, g.dec), ERROR(3)),
     LIMIT BAYESFACTOR TO 1e3) AS x
";

            RunQuery(sql);
        }

        [TestMethod]
        [TestCategory("Query")]
        public void DotInUsernameTest()
        {
            var sql =
@"SELECT [x].[MatchID], [x].[RA], [x].[Dec], [s1].[objID], [s1].[ra], [s1].[dec], [m].[objID], [m].[ra], [m].[dec]
INTO MYDB:[$into]
FROM XMATCH (
    MUST EXIST IN [SDSSDR12]:[dbo].[PhotoObjAll] AS [s1] WITH(ERROR(0.1)),
    MUST EXIST IN [MYDB]:[MyCatalog] AS [m] WITH(ERROR(0.1)),
    LIMIT BAYESFACTOR TO 1000) AS x
REGION 'CIRCLE J2000 0.0 0.0 10.0'";

            RunQuery(sql);
        }
    }
}
