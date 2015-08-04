using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;
using Jhu.SkyQuery.Parser;

namespace Jhu.SkyQuery.Parser.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class CoordinateHintTest : SkyQueryParserTest
    {
        [TestMethod]
        public void SimpleCoordinateHintTest()
        {
            var sql = "dummytable WITH(POINT(ra,dec), ERROR(1.0))";
            var t = (TableSource)Parser.Execute(new TableSource(), sql);

            var coords = new TableCoordinates((SimpleTableSource)t.SpecificTableSource, CodeDataset);
        }

        [TestMethod]
        public void XMatchCoordinateHintsTest()
        {
            var sql =
@"SELECT c1.ra, c1.dec, c2.ra, c2.dec
FROM 
    XMATCH
        (MUST EXIST IN d1:c1 WITH(POINT(c1.ra, c1.dec), ERROR(0.1)),
         MUST EXIST IN d2:c2 WITH(POINT(c2.ra, c2.dec), ERROR(c2.err, 0.1, 0.5)),
         LIMIT BAYESFACTOR TO 1000) AS x";

            var qs = Parse(sql);

            var ts = qs.EnumerateSourceTables(false).ToArray();

            Assert.AreEqual(3, ts.Length);

            var coords = new TableCoordinates((SimpleTableSource)ts[1], CodeDataset);
            Assert.AreEqual("[c1].[ra]", coords.RA);
            Assert.AreEqual("[c1].[dec]", coords.Dec);
            Assert.AreEqual("0.1", coords.Error);
            Assert.IsTrue(coords.IsConstantError);

            coords = new TableCoordinates((SimpleTableSource)ts[2], CodeDataset);
            Assert.AreEqual("[c2].[ra]", coords.RA);
            Assert.AreEqual("[c2].[dec]", coords.Dec);
            Assert.AreEqual("[c2].[err]", coords.Error);
            Assert.AreEqual("0.1", coords.ErrorMin);
            Assert.AreEqual("0.5", coords.ErrorMax);
            Assert.IsFalse(coords.IsConstantError);
        }

        // TODO add more coordinate tests

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
