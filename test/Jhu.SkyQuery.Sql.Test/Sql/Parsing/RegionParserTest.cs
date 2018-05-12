using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.SkyQuery.Sql.Parsing
{
    [TestClass]
    public class RegionParserTest
    {
        protected RegionClause Parse(string query)
        {
            var p = new SkyQueryParser();
            return (RegionClause)p.Execute(new RegionClause(), query);
        }

        [TestMethod]
        public void StringTest()
        {
            var r = Parse("REGION 'here comes a region definition'");

            Assert.IsTrue(r.IsString);
            Assert.IsFalse(r.IsUri);
            Assert.AreEqual("here comes a region definition", r.RegionString);
        }

        [TestMethod]
        public void UriTest()
        {
            var r = Parse("REGION 'http://voservices.net/footprint/api/v1/footprints/sdssdr7'");

            Assert.IsTrue(r.IsString);
            Assert.IsTrue(r.IsUri);
            Assert.AreEqual("http://voservices.net/footprint/api/v1/footprints/sdssdr7", r.RegionString);
            Assert.AreEqual("http://voservices.net/footprint/api/v1/footprints/sdssdr7", r.RegionUri.OriginalString);
        }

        [TestMethod]
        public void CircleTest()
        {
            Parse("REGION CIRCLE(10, 20, 10)");
            Parse("REGION CIRC(10, 20, 10)");
        }

        [TestMethod]
        public void RectangleTest()
        {
            Parse("REGION RECTANGLE(10, 20, 20, 30)");
            Parse("REGION RECT(10, 20, 10, 40)");
        }

        [TestMethod]
        public void PolygonTest()
        {
            Parse("REGION POLYGON(10, 20, 20, 30, 30, 40)");
            Parse("REGION POLY(10, 20, 20, 30, 30, 40)");
        }

        [TestMethod]
        public void ConvexHullTest()
        {
            Parse("REGION CONVEX_HULL(10, 20, 20, 30, 30, 40)");
            Parse("REGION CHULL(10, 20, 20, 30, 30, 40)");
        }

        [TestMethod]
        public void BracketsTest()
        {
            Parse("REGION (CIRC(10, 20, 10))");
            Parse("REGION ((CIRC(10, 20, 10)))");
        }

        [TestMethod]
        public void OperatorsTest()
        {
            Parse("REGION (CIRC(10, 20, 10) UNION CIRC(10, 20, 20))");
            Parse("REGION (CIRC(10, 20, 10)) INTERSECT (CIRC(10, 20, 20))");
            Parse("REGION (CIRC(10, 20, 10)) UNION (CIRC(10, 20, 20)) INTERSECT (RECT(10, 20, 30, 40))");
        }
    }
}
