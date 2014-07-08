﻿using System;
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

namespace Jhu.SkyQuery.Parser.Test
{
    [TestClass]
    public class SkyQueryNameResolverTest
    {
        private SchemaManager CreateSchemaManager()
        {
            return new SqlServerSchemaManager();
        }

        private QuerySpecification Parse(string query)
        {
            var p = new SkyQueryParser();
            var ss = (SelectStatement)p.Execute(new SelectStatement(), query);
            var qs = (SkyQuery.Parser.QuerySpecification)ss.EnumerateQuerySpecifications().First();

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
FROM CatalogA a WITH(POINT(a.cx, a.cy, a.cz)),
CatalogB b WITH(POINT(b.cx, b.cy, b.cz))
XMATCH BAYESFACTOR x
MUST EXIST a
MUST EXIST b
HAVING LIMIT 1e3";

            var qs = Parse(sql);
            var ts = qs.SourceTableReferences.Values.ToArray();
            var xm = qs.FindDescendant<BayesianXMatchClause>();
            var xts = xm.EnumerateXMatchTableSpecifications().ToArray();

            Assert.AreEqual(3, ts.Length);
            Assert.AreEqual("[a]", ts[0].ToString());
            Assert.AreEqual("[b]", ts[1].ToString());
            Assert.AreEqual("[a]", xts[0].TableReference.ToString());
            Assert.AreEqual("[b]", xts[1].TableReference.ToString());
            Assert.IsTrue(ts[2].IsComputed);
        }

        [TestMethod]
        public void InclusionMethodTest()
        {
            var sql =
@"SELECT x.*
FROM CatalogA a WITH(POINT(a.cx, a.cy, a.cz)),
CatalogB b WITH(POINT(b.cx, b.cy, b.cz)),
CatalogC c WITH(POINT(c.cx, c.cy, c.cz)),
CatalogD d WITH(POINT(d.cx, d.cy, d.cz))
XMATCH BAYESFACTOR x
MUST EXIST a
EXIST b
MAY EXIST c
NOT EXIST d
HAVING LIMIT 1e3";

            var qs = Parse(sql);
            var ts = qs.SourceTableReferences.Values.ToArray();
            var xm = qs.FindDescendant<BayesianXMatchClause>();
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
FROM CatalogA a WITH(POINT(a.cx, a.cy, a.cz)),
CatalogB b WITH(POINT(b.cx, b.cy, b.cz))
XMATCH BAYESFACTOR x
MUST EXIST a
MUST EXIST b
HAVING LIMIT 1e3";

            var qs = Parse(sql);

            var res = GenerateCode(qs);

            Assert.AreEqual(
@"SELECT [a].[objId] AS [a_objId], [a].[ra] AS [a_ra], [a].[dec] AS [a_dec],
         [b].[objId] AS [b_objId], [b].[ra] AS [b_ra], [b].[dec] AS [b_dec],
         [x].[LogBF] AS [x_LogBF], [x].[RA] AS [x_RA], [x].[Dec] AS [x_Dec], [x].[Q] AS [x_Q], [x].[L] AS [x_L], [x].[A] AS [x_A], [x].[Cx] AS [x_Cx], [x].[Cy] AS [x_Cy], [x].[Cz] AS [x_Cz]
FROM [SkyNode_Test].[dbo].[CatalogA] [a] WITH(POINT([a].[cx], [a].[cy], [a].[cz])),
[SkyNode_Test].[dbo].[CatalogB] [b] WITH(POINT([b].[cx], [b].[cy], [b].[cz]))
XMATCH BAYESFACTOR [x]
MUST EXIST [SkyNode_Test].[dbo].[CatalogA]
MUST EXIST [SkyNode_Test].[dbo].[CatalogB]
HAVING LIMIT 1e3", res);

        }
    }
}
