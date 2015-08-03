using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.ParserLib;
using Jhu.SkyQuery.Parser;

namespace Jhu.SkyQuery.Parser.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class CoordinateHintTest
    {
        protected QuerySpecification Parse(string query)
        {
            var p = new SkyQueryParser();
            return (QuerySpecification)((SelectStatement)p.Execute(query)).EnumerateQuerySpecifications().First();;
        }

        [TestMethod]
        public void SimpleCoordinateHintTest()
        {
            var sql = "WITH(POINT(ra,dec), ERROR(1.0))";

            var p = new SkyQueryParser();
            var h = p.Execute(new CoordinateHintClause(), sql);
        }

        // TODO add more coordinate tests
        
        [TestMethod]
        public void XMatchCoordinateHintsTest()
        {
            var sql =
@"SELECT c1.ra, c1.dec, c2.ra, c2.dec
FROM 
    XMATCH x AS
    (
        MUST EXIST IN d1:c1 WITH(POINT(c1.ra, c1.dec), ERROR(0.1)),
        MUST EXIST IN d2:c2 WITH(POINT(c2.ra, c2.dec), ERROR(c2.err, 0.1, 0.5))
        LIMIT BAYESFACTOR TO 1000
    )
";

            var qs = Parse(sql);

            var ts = qs.EnumerateSourceTables(false).ToArray();

            Assert.AreEqual(3, ts.Length);

            var cts = (CoordinateTableSource)ts[1];
            Assert.AreEqual("POINT(c1.ra, c1.dec)", cts.Position.ToString());
            Assert.AreEqual("0.1", cts.ErrorExpression.ToString());
            Assert.IsTrue(cts.IsConstantError);

            cts = (CoordinateTableSource)ts[2];
            Assert.AreEqual("POINT(c2.ra, c2.dec)", cts.Position.ToString());
            Assert.AreEqual("c2.err", cts.ErrorExpression.ToString());
            Assert.AreEqual("0.1", cts.MinErrorExpression.ToString());
            Assert.AreEqual("0.5", cts.MaxErrorExpression.ToString());
            Assert.IsFalse(cts.IsConstantError);
        }

        [TestMethod]
        public void RegionQueryTest()
        {
            var sql =
        @"SELECT TOP 100 a.objid, a.ra, a.dec
INTO PartitionedSqlQueryTest_SimpleQueryTest
FROM SDSSDR7:PhotoObjAll a WITH(POINT(ra, dec))
REGION CIRCLE(10, 20, 30)
";
            var qs = Parse(sql);

            Assert.IsTrue(qs.FindAscendant<XMatchSelectStatement>() == null);
            Assert.IsTrue(qs.FindAscendant<RegionSelectStatement>() != null);
        }
    }
}
