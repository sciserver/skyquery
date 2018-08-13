using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.Graywulf.Sql.Extensions.Parsing;

namespace Jhu.SkyQuery.Sql.Parsing
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class SkyQueryParserTest : SkyQueryTestBase
    {
        
        [TestMethod]
        public void SimpleQueryTest()
        {
            var sql =
@"SELECT ra, dec FROM d:c";

            var qs = Parse(sql);
            Assert.IsNull(qs.FindDescendant<XMatchQuerySpecification>());
        }

        [TestMethod]
        public void PartitionedQueryTest()
        {
            var sql =
        @"SELECT TOP 100 a.objid, a.ra, a.dec
INTO PartitionedSqlQueryTest_SimpleQueryTest
FROM SDSSDR7:PhotoObjAll a PARTITION BY a.objid";

            var qs = Parse<PartitionedQuerySpecification>(sql);
        }

        [TestMethod]
        public void RegionQueryTest()
        {
            var sql =
        @"SELECT TOP 100 a.objid, a.ra, a.dec
INTO PartitionedSqlQueryTest_SimpleQueryTest
FROM SDSSDR7:PhotoObjAll a
REGION CIRCLE(20, 30, 10)";
            var qs = Parse(sql);
        }

        [TestMethod]
        public void RegionQueryWithUnionTest1()
        {
            var sql =
@"SELECT TOP 100 a.objid, a.ra, a.dec
FROM SDSSDR7:PhotoObjAll a
REGION CIRCLE(20, 30, 10)
UNION
SELECT TOP 100 a.objid, a.ra, a.dec
FROM SDSSDR7:PhotoObjAll a";

            var qs = Parse(sql);
        }

        [TestMethod]
        public void RegionQueryWithUnionTest2()
        {
            var sql =
@"SELECT TOP 100 a.objid, a.ra, a.dec
FROM SDSSDR7:PhotoObjAll a
REGION CIRCLE(20, 30, 10)
UNION
SELECT TOP 100 a.objid, a.ra, a.dec
FROM SDSSDR7:PhotoObjAll a
REGION CIRCLE(20, 30, 10)";

            var qs = Parse(sql);
        }

        [TestMethod]
        public void RegionQueryWithUnionTest3()
        {
            var sql =
@"SELECT TOP 100 a.objid, a.ra, a.dec
FROM SDSSDR7:PhotoObjAll a
UNION
SELECT TOP 100 a.objid, a.ra, a.dec
FROM SDSSDR7:PhotoObjAll a
REGION CIRCLE(20, 30, 10)";

            var qs = Parse(sql);
        }

        [TestMethod]
        public void ConeXMatchTest()
        {
            var sql =
@"SELECT x.ra, x.dec, s.ra, s.dec, t.ra, t.dec
FROM x AS XMATCH
(
    MUST EXIST IN SDSSDR7:SpecObj AS s,
    MUST EXIST IN TwoMASS:PhotoPSC AS t,
    LIMIT CONE TO CIRCLE(s.RA, s.Dec, 2)
)";

            var qs = Parse(sql);
            Assert.IsNotNull(qs.FindDescendantRecursive<ConeXMatchTableSource>());
        }

        [TestMethod]
        public void BayesFactorXMatchTest()
        {
            var sql =
@"SELECT x.ra, x.dec, s.ra, s.dec, t.ra, t.dec
FROM x AS XMATCH
(
    MUST EXIST IN SDSSDR7:SpecObj AS s,
    MUST EXIST IN TwoMASS:PhotoPSC AS t,
    LIMIT BAYESFACTOR TO 1e3
)";

            var qs = Parse(sql);
            Assert.IsNotNull(qs.FindDescendantRecursive<BayesFactorXMatchTableSource>());
        }

        [TestMethod]
        public void CrossJoinXMatchTest()
        {
            var sql =
@"SELECT m.ra, m.dec, x.ra, x.dec
INTO [$targettable]
FROM XMATCH
     (MUST EXIST IN SDSSDR7:PhotoObjAll AS s WITH(POINT(s.ra, s.dec), ERROR(s.raErr, 0.05, 0.1)),
      MUST EXIST IN MyCatalog m WITH(POINT(m.ra, m.dec), ERROR(0.2)),
      LIMIT BAYESFACTOR TO 1e3) x
WHERE s.ra BETWEEN 0 AND 0.5 AND s.dec BETWEEN 0 AND 0.5";

            var qs = Parse(sql);
        }
    }
}
