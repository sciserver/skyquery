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
        public void TableValuedFunctionTest()
        {
            var sql = 
@"SELECT htm.htmidstart, htm.htmidend
INTO SqlQueryTest_TableValuedFunctionJoinTest
FROM dbo.fHtmCoverCircleEq(100) AS htm";
            var qs = Parse<QuerySpecification>(sql);
        }

        [TestMethod]
        public void JoinedTableValuedFunctionTest()
        {
            var sql =
@"SELECT TOP 100 objid, ra, dec
INTO SqlQueryTest_TableValuedFunctionJoinTest
FROM dbo.fHtmCoverCircleEq (0, 0, 10) AS htm
INNER JOIN SDSSDR7:PhotoObj p
    ON p.htmid BETWEEN htm.htmidstart AND htm.htmidend";

            var qs = Parse<QuerySpecification>(sql);
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

            var qs = Parse<XMatchQuerySpecification>(sql);
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
    }
}
