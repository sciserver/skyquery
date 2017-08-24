using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;
using Jhu.SkyQuery.Parser;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Schema.SqlServer;

namespace Jhu.SkyQuery.Parser
{
    [TestClass]
    public class SkyQueryNameResolverTest : SkyQueryParserTestBase
    {
        private SchemaManager CreateSchemaManager()
        {
            return new SqlServerSchemaManager();
        }

        protected XMatchQuerySpecification Parse(string query)
        {
            var ss = Parser.Execute<XMatchSelectStatement>(query);
            var qs = (XMatchQuerySpecification)ss.EnumerateQuerySpecifications().First();

            var nr = new SkyQueryNameResolver();
            nr.DefaultTableDatasetName = Jhu.Graywulf.Test.Constants.TestDatasetName;
            nr.DefaultFunctionDatasetName = Jhu.Graywulf.Test.Constants.CodeDatasetName;
            nr.SchemaManager = CreateSchemaManager();
            nr.Execute(ss);

            return qs;
        }

        private string GenerateCode(QuerySpecification qs)
        {
            var cg = new Jhu.Graywulf.SqlCodeGen.SqlServer.SqlServerCodeGenerator();
            cg.ResolveNames = true;

            var sw = new StringWriter();
            cg.Execute(sw, qs);

            return sw.ToString();
        }

        [TestMethod]
        public void SimpleQueryTest()
        {
            var sql = "SELECT objId, ra, dec FROM CatalogA";

            var qs = Parse(sql);
            var ts = qs.SourceTableReferences.Values.ToArray();

            Assert.AreEqual("CatalogA", ts[0].DatabaseObjectName);
        }

        [TestMethod]
        public void XMatchQueryTest()
        {
            var sql =
@"SELECT a.objID, a.ra, a.dec,
         b.objID, b.ra, b.dec,
         x.ra, x.dec
FROM XMATCH
    (MUST EXIST IN CatalogA a WITH(POINT(a.cx, a.cy, a.cz)),
     MUST EXIST IN CatalogB b WITH(POINT(b.cx, b.cy, b.cz)),
     LIMIT BAYESFACTOR TO 1000) AS x";

            var qs = Parse(sql);
            var ts = qs.SourceTableReferences.Values.ToArray();
            var xm = qs.FindDescendantRecursive<BayesFactorXMatchTableSource>();
            var xts = xm.EnumerateXMatchTableSpecifications().ToArray();

            Assert.AreEqual(3, ts.Length);

            Assert.AreEqual("[x]", ts[0].ToString());
            Assert.IsTrue(ts[0].IsComputed);

            Assert.AreEqual("[a]", ts[1].ToString());
            Assert.AreEqual("[b]", ts[2].ToString());
            Assert.AreEqual("[a]", xts[0].TableReference.ToString());
            Assert.AreEqual("[b]", xts[1].TableReference.ToString());
        }

        [TestMethod]
        public void XMatchQueryWithValidTableHintsTest()
        {
            var sql =
@"SELECT a.objID, a.ra, a.dec,
         b.objID, b.ra, b.dec,
         x.ra, x.dec
FROM XMATCH
    (MUST EXIST IN CatalogA a WITH(POINT(cx, cy, cz), HTMID(htmID)),
     MUST EXIST IN CatalogB b WITH(POINT(cx, cy, cz), HTMID(htmID)),
     LIMIT BAYESFACTOR TO 1000) AS x";

            var qs = Parse(sql);
            var ts = qs.SourceTableReferences.Values.ToArray();
            var xm = qs.FindDescendantRecursive<BayesFactorXMatchTableSource>();
            var xts = xm.EnumerateXMatchTableSpecifications().ToArray();

            Assert.AreEqual("[a].[cx]", CodeGenerator.Execute(xts[0].Coordinates.XHintExpression));
            Assert.AreEqual("[b].[cx]", CodeGenerator.Execute(xts[1].Coordinates.XHintExpression));
            Assert.AreEqual("[a].[htmId]", CodeGenerator.Execute(xts[0].Coordinates.HtmIdHintExpression));
            Assert.AreEqual("[b].[htmId]", CodeGenerator.Execute(xts[1].Coordinates.HtmIdHintExpression));
        }

        // TODO: add test for zoneID, but need to modify catalog schema first

        [TestMethod]
        public void XMatchQueryWithInvalidTableHintsTest()
        {
            var sql =
@"SELECT a.objID, a.ra, a.dec,
         b.objID, b.ra, b.dec,
         x.ra, x.dec
FROM XMATCH
    (MUST EXIST IN CatalogA a WITH(POINT(b.cx, b.cy, b.cz)),
     MUST EXIST IN CatalogB b WITH(POINT(b.cx, b.cy, b.cz)),
     LIMIT BAYESFACTOR TO 1000) AS x";

            try
            {
                var qs = Parse(sql);
                Assert.Fail();
            }
            catch (NameResolverException)
            {
            }
        }


        [TestMethod]
        public void XMatchQueryWithHtmIdHintTest()
        {
            var sql =
@"SELECT a.objID, a.ra, a.dec,
         b.objID, b.ra, b.dec,
         x.ra, x.dec
FROM XMATCH
    (MUST EXIST IN CatalogA a WITH(POINT(cx, cy, cz), HTMID(htmID)),
     MUST EXIST IN CatalogB b WITH(POINT(cx, cy, cz), HTMID(htmID)),
     LIMIT BAYESFACTOR TO 1000) AS x";

            var qs = Parse(sql);
            var ts = qs.SourceTableReferences.Values.ToArray();
            var xm = qs.FindDescendantRecursive<BayesFactorXMatchTableSource>();
            var xts = xm.EnumerateXMatchTableSpecifications().ToArray();

            Assert.AreEqual("[a].[cx]", CodeGenerator.Execute(xts[0].Coordinates.XHintExpression));
            Assert.AreEqual("[b].[cx]", CodeGenerator.Execute(xts[1].Coordinates.XHintExpression));
            Assert.AreEqual("[a].[htmId]", CodeGenerator.Execute(xts[0].Coordinates.HtmIdHintExpression));
            Assert.AreEqual("[b].[htmId]", CodeGenerator.Execute(xts[1].Coordinates.HtmIdHintExpression));
        }

        [TestMethod]
        public void XMatchQueryWithZoneIdHintTest()
        {
            var sql =
@"SELECT a.objID, a.ra, a.dec,
         b.objID, b.ra, b.dec,
         x.ra, x.dec
FROM XMATCH
    (MUST EXIST IN CatalogA a WITH(POINT(cx, cy, cz), ZONEID(zoneID)),
     MUST EXIST IN CatalogB b WITH(POINT(cx, cy, cz), ZONEID(zoneID)),
     LIMIT BAYESFACTOR TO 1000) AS x";

            var qs = Parse(sql);
            var ts = qs.SourceTableReferences.Values.ToArray();
            var xm = qs.FindDescendantRecursive<BayesFactorXMatchTableSource>();
            var xts = xm.EnumerateXMatchTableSpecifications().ToArray();

            Assert.AreEqual("[a].[cx]", CodeGenerator.Execute(xts[0].Coordinates.XHintExpression));
            Assert.AreEqual("[b].[cx]", CodeGenerator.Execute(xts[1].Coordinates.XHintExpression));
            Assert.AreEqual("[a].[zoneId]", CodeGenerator.Execute(xts[0].Coordinates.ZoneIdHintExpression));
            Assert.AreEqual("[b].[zoneId]", CodeGenerator.Execute(xts[1].Coordinates.ZoneIdHintExpression));
        }
        
        [TestMethod]
        public void InclusionMethodTest()
        {
            var sql =
@"SELECT x.*
FROM XMATCH
    (MUST EXIST IN CatalogA a WITH(POINT(a.cx, a.cy, a.cz)),
     MUST EXIST IN CatalogB b WITH(POINT(b.cx, b.cy, b.cz)),
     MAY EXIST IN CatalogC c WITH(POINT(c.cx, c.cy, c.cz)),
     NOT EXIST IN CatalogD d WITH(POINT(d.cx, d.cy, d.cz)),
     LIMIT BAYESFACTOR TO 1e3) AS x
";

            var qs = Parse(sql);
            var ts = qs.SourceTableReferences.Values.ToArray();
            var xm = qs.FindDescendantRecursive<BayesFactorXMatchTableSource>();
            var xts = xm.EnumerateXMatchTableSpecifications().ToArray();

            Assert.AreEqual(XMatchInclusionMethod.Must, xts[0].InclusionMethod);
            Assert.AreEqual(XMatchInclusionMethod.Must, xts[1].InclusionMethod);
            Assert.AreEqual(XMatchInclusionMethod.May, xts[2].InclusionMethod);
            Assert.AreEqual(XMatchInclusionMethod.Drop, xts[3].InclusionMethod);
        }

        [TestMethod]
        public void SelectStarTest()
        {
            var sql =
@"SELECT a.objID, a.ra, a.dec,
         b.objID, b.ra, b.dec,
         x.*
FROM XMATCH
    (MUST EXIST IN CatalogA a WITH(POINT(a.cx, a.cy, a.cz)),
     MUST EXIST IN CatalogB b WITH(POINT(b.cx, b.cy, b.cz)),
     LIMIT BAYESFACTOR TO 1e3) AS x";

            var gt =
@"SELECT [a].[objId] AS [a_objId], [a].[ra] AS [a_ra], [a].[dec] AS [a_dec],
         [b].[objId] AS [b_objId], [b].[ra] AS [b_ra], [b].[dec] AS [b_dec],
         [x].[MatchID] AS [x_MatchID], [x].[RA] AS [x_RA], [x].[Dec] AS [x_Dec], [x].[Cx] AS [x_Cx], [x].[Cy] AS [x_Cy], [x].[Cz] AS [x_Cz], [x].[N] AS [x_N], [x].[A] AS [x_A], [x].[L] AS [x_L], [x].[Q] AS [x_Q], [x].[LogBF] AS [x_LogBF]
FROM XMATCH
    (MUST EXIST IN [SkyNode_Test].[dbo].[CatalogA] [a] WITH(POINT([a].[cx], [a].[cy], [a].[cz])),
     MUST EXIST IN [SkyNode_Test].[dbo].[CatalogB] [b] WITH(POINT([b].[cx], [b].[cy], [b].[cz])),
     LIMIT BAYESFACTOR TO 1e3) AS [x]";

            var qs = Parse(sql);

            var res = GenerateCode(qs);

            Assert.AreEqual(gt, res);
        }

    }
}
