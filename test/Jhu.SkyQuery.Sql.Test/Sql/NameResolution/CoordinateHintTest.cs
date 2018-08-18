using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.SkyQuery.Sql.Parsing;

namespace Jhu.SkyQuery.Sql.NameResolution
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class CoordinateHintTest : SkyQueryTestBase
    {
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            StartLogger();
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            StopLogger();
        }

        [TestMethod]
        public void SimpleCoordinateHintTest()
        {
            var sql =
@"SELECT ra, dec
FROM SDSSDR7:PhotoObj WITH(POINT(ra,dec), ERROR(1.0))
REGION 'CIRCLE J2000 10 10 0'";

            var ss = ParseAndResolveNames<SelectStatement>(sql);
            var qs = ss.QueryExpression.EnumerateQuerySpecifications().FirstOrDefault();
            var t = (TableSourceSpecification)qs.FindDescendantRecursive<TableSourceSpecification>();

            var coords = ((CoordinatesTableSource)qs.SourceTableReferences["PhotoObj"].TableSource).Coordinates;
            Assert.AreEqual("[c1].[ra]", QueryRenderer.Execute(coords.RAHintExpression));
            Assert.AreEqual("[c1].[dec]", QueryRenderer.Execute(coords.DecHintExpression));
            Assert.AreEqual("0.1", QueryRenderer.Execute(coords.ErrorHintExpression));
            Assert.IsTrue(coords.IsConstantError);
        }

        [TestMethod]
        public void XMatchCoordinateHintsTest()
        {
            var sql =
@"SELECT c1.ra, c1.dec, c2.ra, c2.dec
FROM 
    XMATCH
        (MUST EXIST IN SDSSDR7:SpecObj AS c1 WITH(POINT(c1.ra, c1.dec), ERROR(0.1)),
         MUST EXIST IN SDSSDR7:PhotoObj AS c2 WITH(POINT(c2.ra, c2.dec), ERROR(c2.raErr, 0.1, 0.5)),
         LIMIT BAYESFACTOR TO 1000) AS x";

            var qs = ParseAndResolveNames<XMatchQuerySpecification>(sql);
            var ts = qs.SourceTableReferences.Values.ToArray();

            Assert.AreEqual(3, ts.Length);

            var coords = new TableCoordinates((CoordinatesTableSource)ts[1].TableSource);
            Assert.AreEqual("[c1].[ra]", QueryRenderer.Execute(coords.RAHintExpression));
            Assert.AreEqual("[c1].[dec]", QueryRenderer.Execute(coords.DecHintExpression));
            Assert.AreEqual("0.1", QueryRenderer.Execute(coords.ErrorHintExpression));
            Assert.IsTrue(coords.IsConstantError);

            coords = new TableCoordinates((CoordinatesTableSource)ts[2].TableSource);
            Assert.AreEqual("[c2].[ra]", QueryRenderer.Execute(coords.RAHintExpression));
            Assert.AreEqual("[c2].[dec]", QueryRenderer.Execute(coords.DecHintExpression));
            Assert.AreEqual("[c2].[err]", QueryRenderer.Execute(coords.ErrorHintExpression));
            Assert.AreEqual("0.1", coords.ErrorHintMinExpression.Value);
            Assert.AreEqual("0.5", coords.ErrorHintMaxExpression.Value);
            Assert.IsFalse(coords.IsConstantError);
        }

        [TestMethod]
        public void CoordinatesWithHtmIdTest()
        {
            var sql =
@"SELECT c1.ra, c1.dec, c2.ra, c2.dec
FROM 
    XMATCH
        (MUST EXIST IN SDSSDR7:PhotoObj AS c1 WITH(POINT(c1.ra, c1.dec), ERROR(0.1), HTMID(c1.htmID)),
         MUST EXIST IN TwoMASS:PhotoPSC AS c2 WITH(POINT(c2.ra, c2.dec), ERROR(c2.err_ang, 0.1, 0.5), HTMID(c2.htmID)),
         LIMIT BAYESFACTOR TO 1000) AS x";

            var qs = ParseAndResolveNames<XMatchQuerySpecification>(sql);

            var ts = qs.SourceTableReferences.Values.ToArray();

            Assert.AreEqual(3, ts.Length);

            var coords = ((CoordinatesTableSource)ts[1].TableSource).Coordinates;
            Assert.AreEqual("[c1].[ra]", QueryRenderer.Execute(coords.RAHintExpression));
            Assert.AreEqual("[c1].[dec]", QueryRenderer.Execute(coords.DecHintExpression));
            Assert.AreEqual("[c1].[htmID]", QueryRenderer.Execute(coords.HtmIdHintExpression));

            coords = ((CoordinatesTableSource)ts[2].TableSource).Coordinates;
            Assert.AreEqual("[c2].[htmid]", QueryRenderer.Execute(coords.HtmIdHintExpression));
        }

        [TestMethod]
        public void CoordinatesWithZoneIdTest()
        {

            var sql =
    @"SELECT c1.ra, c1.dec, c2.ra, c2.dec
FROM 
    XMATCH
        (MUST EXIST IN SDSSDR7:PhotoObj c1 WITH(POINT(c1.ra, c1.dec), ERROR(0.1), ZONEID(c1.zoneID)),
         MUST EXIST IN TwoMASS:PhotoPSC c2 WITH(POINT(c2.ra, c2.dec), ERROR(c2.err_ang, 0.1, 0.5), ZONEID(c2.zoneID)),
         LIMIT BAYESFACTOR TO 1000) AS x";

            var qs = ParseAndResolveNames<XMatchQuerySpecification>(sql);
            var ts = qs.SourceTableReferences.Values.ToArray();

            Assert.AreEqual(3, ts.Length);

            var coords =((CoordinatesTableSource)ts[1].TableSource).Coordinates;
            Assert.AreEqual("[c1].[ra]", QueryRenderer.Execute(coords.RAHintExpression));
            Assert.AreEqual("[c1].[dec]", QueryRenderer.Execute(coords.DecHintExpression));
            Assert.AreEqual("[c1].[zoneID]", QueryRenderer.Execute(coords.ZoneIdHintExpression));

            coords = ((CoordinatesTableSource)ts[2].TableSource).Coordinates;
            Assert.AreEqual("[c2].[zoneid]", QueryRenderer.Execute(coords.ZoneIdHintExpression));
        }

        [TestMethod]
        public void SimpleRegionQueryTest()
        {
            var sql =
        @"SELECT TOP 100 a.objid, a.ra, a.dec
INTO PartitionedSqlQueryTest_SimpleQueryTest
FROM SDSSDR7:PhotoObjAll a WITH(POINT(ra, dec))
REGION CIRCLE(10, 20, 30)
";
            var qs = Parse(sql);

            Assert.IsTrue(qs.FindAscendant<XMatchSelectStatement>() == null);
            //Assert.IsTrue(qs.FindAscendant<RegionSelectStatement>() != null);
        }
    }
}
