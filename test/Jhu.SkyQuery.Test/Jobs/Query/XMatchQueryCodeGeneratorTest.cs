using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Schema.SqlServer;
using Jhu.Graywulf.SqlCodeGen;
using Jhu.Graywulf.SqlCodeGen.SqlServer;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Jobs.Query;
using Jhu.SkyQuery.Parser;

namespace Jhu.SkyQuery.Jobs.Query.Test
{
    [TestClass]
    public class XMatchQueryCodeGeneratorTest : SkyQueryTestBase
    {
        protected XMatchQueryCodeGenerator CodeGenerator
        {
            get
            {
                return new XMatchQueryCodeGenerator()
                {
                    CodeDataset = new Graywulf.Schema.SqlServer.SqlServerDataset()
                    {
                        Name = "CODE",
                        ConnectionString = "data source=localhost;initial catalog=SkyQuery_Code",
                    }
                };
            }
        }

        protected override SkyQuery.Parser.SelectStatement Parse(string sql)
        {
            var p = new SkyQueryParser();
            var ss = (SkyQuery.Parser.SelectStatement)p.Execute(new SkyQuery.Parser.SelectStatement(), sql);
            var qs = (SkyQuery.Parser.QuerySpecification)ss.EnumerateQuerySpecifications().First();

            var nr = new SkyQueryNameResolver();
            nr.DefaultTableDatasetName = Jhu.Graywulf.Test.Constants.TestDatasetName;
            nr.DefaultFunctionDatasetName = Jhu.Graywulf.Test.Constants.CodeDatasetName;
            nr.SchemaManager = CreateSchemaManager();
            nr.Execute(ss);

            return ss;
        }

        #region Helper functions

        private string[] GeneratePropagatedColumnListTestHelper(string sql, ColumnContext context)
        {
            var res = new List<string>();
            var q = CreateQuery(sql);
            var cg = new XMatchQueryCodeGenerator(q);

            foreach (var qs in q.SelectStatement.EnumerateQuerySpecifications())
            {
                foreach (var ts in qs.EnumerateSourceTables(false))
                {
                    var columns = new SqlServerColumnListGenerator(ts.TableReference, context, ColumnListType.ForSelectWithEscapedNameNoAlias)
                    {
                        TableAlias = "tablealias",
                    };

                    res.Add(columns.GetColumnListString());
                }
            }

            return res.ToArray();
        }

        private string GetExecuteQueryTextTestHelper(string sql)
        {
            using (var context = ContextManager.Instance.CreateContext(ConnectionMode.AutoOpen, TransactionMode.AutoCommit))
            {
                var sm = new GraywulfSchemaManager(context.Federation);

                var xmq = CreateQuery(sql);
                xmq.ExecutionMode = ExecutionMode.SingleServer;

                var xmqp = new BayesFactorXMatchQueryPartition((BayesFactorXMatchQuery)xmq);
                xmqp.ID = 0;
                xmqp.InitializeQueryObject(null);
                xmqp.DefaultDataset = (SqlServerDataset)sm.Datasets[Jhu.Graywulf.Test.Constants.TestDatasetName];
                xmqp.CodeDataset = (SqlServerDataset)sm.Datasets[Jhu.Graywulf.Registry.Constants.CodeDbName];

                // TODO: where to take this? let's just hack it for now
                xmqp.TemporaryDataset = new SqlServerDataset()
                {
                    Name = Jhu.Graywulf.Registry.Constants.TempDbName,
                    DatabaseName = "Graywulf_Temp"
                };

                var qs = xmqp.Query.SelectStatement.EnumerateQuerySpecifications().First();
                var fc = qs.FindDescendant<Jhu.Graywulf.SqlParser.FromClause>();
                var xmtables = qs.FindDescendantRecursive<Jhu.SkyQuery.Parser.XMatchTableSource>().EnumerateXMatchTableSpecifications().ToArray();
                xmqp.GenerateSteps(xmtables);

                var cg = new XMatchQueryCodeGenerator(xmqp);

                var res = cg.GetExecuteQuery(xmq.SelectStatement);

                return res.Query;
            }
        }

        #endregion
        #region Propagated columns tests

        [TestMethod]
        public void GetPropagatedColumnListTest()
        {
            // TODO fix this once column propagation works with full query tests

            var sql =
@"SELECT a.objID, a.ra, a.dec,
         b.objID, b.ra, b.dec,
         x.ra, x.dec
FROM XMATCH
    (MUST EXIST IN TEST:CatalogA a WITH(POINT(a.cx, a.cy, a.cz)),
     MUST EXIST IN TEST:CatalogB b WITH(POINT(b.cx, b.cy, b.cz)),
     LIMIT BAYESFACTOR TO 1000) AS x";

            var res = GeneratePropagatedColumnListTestHelper(sql, ColumnContext.Default);
            Assert.AreEqual("[tablealias].[_TEST_dbo_CatalogA_a_objId], [tablealias].[_TEST_dbo_CatalogA_a_ra], [tablealias].[_TEST_dbo_CatalogA_a_dec]", res[1]);

            res = GeneratePropagatedColumnListTestHelper(sql, ColumnContext.PrimaryKey);
            Assert.AreEqual("[tablealias].[_TEST_dbo_CatalogA_a_objId]", res[1]);

            res = GeneratePropagatedColumnListTestHelper(sql, ColumnContext.AllReferenced);
            Assert.AreEqual("[tablealias].[_TEST_dbo_CatalogA_a_objId], [tablealias].[_TEST_dbo_CatalogA_a_ra], [tablealias].[_TEST_dbo_CatalogA_a_dec], [tablealias].[_TEST_dbo_CatalogA_a_cx], [tablealias].[_TEST_dbo_CatalogA_a_cy], [tablealias].[_TEST_dbo_CatalogA_a_cz]", res[1]);
        }

        [TestMethod]
        public void GetPropagatedColumnListTest2()
        {
            // TODO fix this once column propagation works with full query tests

            var sql =
@"SELECT a.ra
FROM XMATCH
    (MUST EXIST IN TEST:CatalogA a WITH(POINT(a.ra, a.dec)),
     MUST EXIST IN TEST:CatalogB b WITH(POINT(b.cx, b.cy, b.cz)),
     LIMIT BAYESFACTOR TO 1e3) AS x";

            var res = GeneratePropagatedColumnListTestHelper(sql, ColumnContext.Default);
            Assert.AreEqual("[tablealias].[_TEST_dbo_CatalogA_a_ra]", res[1]);

            res = GeneratePropagatedColumnListTestHelper(sql, ColumnContext.PrimaryKey);
            Assert.AreEqual("[tablealias].[_TEST_dbo_CatalogA_a_objId]", res[1]);

            res = GeneratePropagatedColumnListTestHelper(sql, ColumnContext.AllReferenced);
            Assert.AreEqual("[tablealias].[_TEST_dbo_CatalogA_a_objId], [tablealias].[_TEST_dbo_CatalogA_a_ra], [tablealias].[_TEST_dbo_CatalogA_a_dec]", res[1]);
        }

        [TestMethod]
        public void GetPropagatedColumnListWithAliasesTest()
        {
            // TODO fix this once column propagation works with full query tests

            var sql =
@"SELECT a.ra
FROM XMATCH
    (MUST EXIST IN TEST:CatalogA a WITH(POINT(a.ra, a.dec)),
     MUST EXIST IN TEST:CatalogB b WITH(POINT(b.cx, b.cy, b.cz)),
     LIMIT BAYESFACTOR TO 1e3) AS x";

            var res = GeneratePropagatedColumnListTestHelper(sql, ColumnContext.Default);
            Assert.AreEqual("[tablealias].[_TEST_dbo_CatalogA_a_ra]", res[1]);

            res = GeneratePropagatedColumnListTestHelper(sql, ColumnContext.PrimaryKey);
            Assert.AreEqual("[tablealias].[_TEST_dbo_CatalogA_a_objId]", res[1]);

            res = GeneratePropagatedColumnListTestHelper(sql, ColumnContext.AllReferenced);
            Assert.AreEqual("[tablealias].[_TEST_dbo_CatalogA_a_objId], [tablealias].[_TEST_dbo_CatalogA_a_ra], [tablealias].[_TEST_dbo_CatalogA_a_dec]", res[1]);
        }

        #endregion
        #region Index selection tests

        [TestMethod]
        public void XMatchQueryWithHtmIndexTest()
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
            var tts = qs.EnumerateSourceTables(false).ToArray();

            var coords = new TableCoordinates((SkyQuery.Parser.SimpleTableSource)tts[1]);
            Assert.IsNotNull(CodeGenerator.FindHtmIndex(coords));

            coords = new TableCoordinates((SkyQuery.Parser.SimpleTableSource)tts[2]);
            Assert.IsNotNull(CodeGenerator.FindHtmIndex(coords));
        }

        [TestMethod]
        public void XMatchQueryWithoutHtmIndexTest()
        {
            var sql =
@"SELECT a.objID, a.ra, a.dec,
         b.objID, b.ra, b.dec,
         x.ra, x.dec
FROM XMATCH
    (MUST EXIST IN [CatalogWithNoPrimaryKey] a WITH(POINT(cx, cy, cz), HTMID(htmID)),
     MUST EXIST IN [CatalogWithNoPrimaryKey] b WITH(POINT(cx, cy, cz), HTMID(htmID)),
     LIMIT BAYESFACTOR TO 1000) AS x";

            var qs = Parse(sql);
            var tts = qs.EnumerateSourceTables(false).ToArray();

            var coords = new TableCoordinates((SkyQuery.Parser.SimpleTableSource)tts[1]);
            Assert.IsNull(CodeGenerator.FindHtmIndex(coords));

            coords = new TableCoordinates((SkyQuery.Parser.SimpleTableSource)tts[2]);
            Assert.IsNull(CodeGenerator.FindHtmIndex(coords));
        }

        [TestMethod]
        public void XMatchQueryWithZoneIndexTest()
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
            var tts = qs.EnumerateSourceTables(false).ToArray();

            var coords = new TableCoordinates((SkyQuery.Parser.SimpleTableSource)tts[1]);
            Assert.IsNotNull(CodeGenerator.FindZoneIndex(coords));

            coords = new TableCoordinates((SkyQuery.Parser.SimpleTableSource)tts[2]);
            Assert.IsNotNull(CodeGenerator.FindZoneIndex(coords));
        }

        [TestMethod]
        public void XMatchQueryWithoutZoneIndexTest()
        {
            var sql =
@"SELECT a.objID, a.ra, a.dec,
         b.objID, b.ra, b.dec,
         x.ra, x.dec
FROM XMATCH
    (MUST EXIST IN CatalogWithNoPrimaryKey a WITH(POINT(cx, cy, cz), ZONEID(zoneID)),
     MUST EXIST IN CatalogWithNoPrimaryKey b WITH(POINT(cx, cy, cz), ZONEID(zoneID)),
     LIMIT BAYESFACTOR TO 1000) AS x";

            var qs = Parse(sql);
            var tts = qs.EnumerateSourceTables(false).ToArray();

            var coords = new TableCoordinates((SkyQuery.Parser.SimpleTableSource)tts[1]);
            Assert.IsNull(CodeGenerator.FindZoneIndex(coords));

            coords = new TableCoordinates((SkyQuery.Parser.SimpleTableSource)tts[2]);
            Assert.IsNull(CodeGenerator.FindZoneIndex(coords));
        }

        #endregion
        #region Output query tests

        [TestMethod]
        public void GetOutputSelectQuery_AllHints_Test()
        {
            var sql =
@"SELECT a.objID, a.ra, a.dec,
b.objID, b.ra, b.dec,
x.ra, x.dec
FROM XMATCH
    (MUST EXIST IN TEST:CatalogA a WITH(POINT(a.cx, a.cy, a.cz)),
     MUST EXIST IN TEST:CatalogB b WITH(POINT(b.cx, b.cy, b.cz)),
     LIMIT BAYESFACTOR TO 1e3) AS x";

            var gt =
@"SELECT [__match].[_TEST_dbo_CatalogA_a_objId] AS [a_objId], [__match].[_TEST_dbo_CatalogA_a_ra] AS [a_ra], [__match].[_TEST_dbo_CatalogA_a_dec] AS [a_dec],
[__match].[_TEST_dbo_CatalogB_b_objId] AS [b_objId], [__match].[_TEST_dbo_CatalogB_b_ra] AS [b_ra], [__match].[_TEST_dbo_CatalogB_b_dec] AS [b_dec],
[__match].[RA] AS [x_RA], [__match].[Dec] AS [x_Dec]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_1] AS [__match]";

            var res = GetExecuteQueryTextTestHelper(sql);
            Assert.AreEqual(gt, res);
        }

        [TestMethod]
        public void GetOutputSelectQuery_NoHints_Test()
        {
            var sql =
@"SELECT a.objID, a.ra, a.dec,
b.objID, b.ra, b.dec,
x.ra, x.dec
FROM XMATCH
    (MUST EXIST IN TEST:CatalogA a,
     MUST EXIST IN TEST:CatalogB b,
     LIMIT BAYESFACTOR TO 1e3) AS x";

            var gt =
@"SELECT [__match].[_TEST_dbo_CatalogA_a_objId] AS [a_objId], [__match].[_TEST_dbo_CatalogA_a_ra] AS [a_ra], [__match].[_TEST_dbo_CatalogA_a_dec] AS [a_dec],
[__match].[_TEST_dbo_CatalogB_b_objId] AS [b_objId], [__match].[_TEST_dbo_CatalogB_b_ra] AS [b_ra], [__match].[_TEST_dbo_CatalogB_b_dec] AS [b_dec],
[__match].[RA] AS [x_RA], [__match].[Dec] AS [x_Dec]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_1] AS [__match]";

            var res = GetExecuteQueryTextTestHelper(sql);
            Assert.AreEqual(gt, res);
        }

        [TestMethod]
        public void GetOutputSelectQuery_ThreeTablesTest()
        {
            var sql =
@"SELECT a.objID, a.ra, a.dec,
         b.objID, b.ra, b.dec,
         x.ra, x.dec
FROM XMATCH
     (MUST EXIST IN TEST:CatalogA a,
      MUST EXIST IN TEST:CatalogB b,
      MUST EXIST IN TEST:CatalogC c,
      LIMIT BAYESFACTOR TO 1e3) AS x";

            var gt =
@"SELECT [__match].[_TEST_dbo_CatalogA_a_objId] AS [a_objId], [__match].[_TEST_dbo_CatalogA_a_ra] AS [a_ra], [__match].[_TEST_dbo_CatalogA_a_dec] AS [a_dec],
         [__match].[_TEST_dbo_CatalogB_b_objId] AS [b_objId], [__match].[_TEST_dbo_CatalogB_b_ra] AS [b_ra], [__match].[_TEST_dbo_CatalogB_b_dec] AS [b_dec],
         [__match].[RA] AS [x_RA], [__match].[Dec] AS [x_Dec]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_2] AS [__match]";

            Assert.AreEqual(gt, GetExecuteQueryTextTestHelper(sql));
        }

        [TestMethod]
        public void GetOutputSelectQuery_CombinedJoinTest()
        {
            var sql =
@"SELECT a.objID, a.ra, a.dec,
         b.objID, b.ra, b.dec,
         c.objID, c.ra, c.dec,
         x.ra, x.dec
FROM XMATCH
     (MUST EXIST IN TEST:CatalogA a,
      MUST EXIST IN TEST:CatalogB b,
      LIMIT BAYESFACTOR TO 1e3) AS x
CROSS JOIN TEST:CatalogC c";

            var gt =
@"SELECT [__match].[_TEST_dbo_CatalogA_a_objId] AS [a_objId], [__match].[_TEST_dbo_CatalogA_a_ra] AS [a_ra], [__match].[_TEST_dbo_CatalogA_a_dec] AS [a_dec],
         [__match].[_TEST_dbo_CatalogB_b_objId] AS [b_objId], [__match].[_TEST_dbo_CatalogB_b_ra] AS [b_ra], [__match].[_TEST_dbo_CatalogB_b_dec] AS [b_dec],
         [c].[objId] AS [c_objId], [c].[ra] AS [c_ra], [c].[dec] AS [c_dec],
         [__match].[RA] AS [x_RA], [__match].[Dec] AS [x_Dec]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_1] AS [__match]
CROSS JOIN [SkyNode_Test].[dbo].[CatalogC] [c]";

            Assert.AreEqual(gt, GetExecuteQueryTextTestHelper(sql));
        }

        [TestMethod]
        public void GetOutputSelectQuery_InnerJoinTest()
        {
            var sql =
@"SELECT a.objID, a.ra, a.dec,
         b.objID, b.ra, b.dec,
         c.objID, c.ra, c.dec,
         x.ra, x.dec
FROM XMATCH
     (MUST EXIST IN TEST:CatalogA a,
      MUST EXIST IN TEST:CatalogB b,
      LIMIT BAYESFACTOR TO 1e3) AS x
INNER JOIN TEST:CatalogC c ON c.objId = a.objId";

            var gt =
@"SELECT [__match].[_TEST_dbo_CatalogA_a_objId] AS [a_objId], [__match].[_TEST_dbo_CatalogA_a_ra] AS [a_ra], [__match].[_TEST_dbo_CatalogA_a_dec] AS [a_dec],
         [__match].[_TEST_dbo_CatalogB_b_objId] AS [b_objId], [__match].[_TEST_dbo_CatalogB_b_ra] AS [b_ra], [__match].[_TEST_dbo_CatalogB_b_dec] AS [b_dec],
         [c].[objId] AS [c_objId], [c].[ra] AS [c_ra], [c].[dec] AS [c_dec],
         [__match].[RA] AS [x_RA], [__match].[Dec] AS [x_Dec]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_1] AS [__match]
INNER JOIN [SkyNode_Test].[dbo].[CatalogC] [c] ON [c].[objId] = [__match].[_TEST_dbo_CatalogA_a_objId]";

            var res = GetExecuteQueryTextTestHelper(sql);

            Assert.AreEqual(gt, res);
        }

        [TestMethod]
        public void GetOutputSelectQuery_WhereTest()
        {
            var sql =
@"SELECT a.objID, b.objID
FROM XMATCH
     (MUST EXIST IN TEST:CatalogA a,
      MUST EXIST IN TEST:CatalogB b,
      LIMIT BAYESFACTOR TO 1e3) AS x
WHERE a.ra BETWEEN 1 AND 2";

            var gt =
@"SELECT [__match].[_TEST_dbo_CatalogA_a_objId] AS [a_objId], [__match].[_TEST_dbo_CatalogB_b_objId] AS [b_objId]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_1] AS [__match]
WHERE [__match].[_TEST_dbo_CatalogA_a_ra] BETWEEN 1 AND 2";

            var res = GetExecuteQueryTextTestHelper(sql);

            Assert.AreEqual(gt, res);
        }

        [TestMethod]
        public void GetOutputSelectQuery_WhereTest2()
        {
            var sql =
@"SELECT a.objID, b.objID
FROM XMATCH
     (MUST EXIST IN TEST:CatalogA a,
      MUST EXIST IN TEST:CatalogB b,
      LIMIT BAYESFACTOR TO 1e3) AS x,
     TEST:CatalogC c
WHERE c.ra BETWEEN 1 AND 2";

            var gt =
@"SELECT [__match].[_TEST_dbo_CatalogA_a_objId] AS [a_objId], [__match].[_TEST_dbo_CatalogB_b_objId] AS [b_objId]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_1] AS [__match],
     [SkyNode_Test].[dbo].[CatalogC] [c]
WHERE [c].[ra] BETWEEN 1 AND 2";

            var res = GetExecuteQueryTextTestHelper(sql);

            Assert.AreEqual(gt, res);

        }

        [TestMethod]
        public void GetOutputSelectQuery_SubqueryTest()
        {
            var sql =
@"SELECT a.objID, b.objID, c.objID
FROM XMATCH
     (MUST EXIST IN TEST:CatalogA a,
      MUST EXIST IN TEST:CatalogB b,
      LIMIT BAYESFACTOR TO 1e3) AS x,
     (SELECT * FROM TEST:CatalogC) c
WHERE c.ra BETWEEN 1 AND 2";

            var gt =
@"SELECT [__match].[_TEST_dbo_CatalogA_a_objId] AS [a_objId], [__match].[_TEST_dbo_CatalogB_b_objId] AS [b_objId], [c].[objId] AS [c_objId]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_1] AS [__match],
     (SELECT [SkyNode_Test].[dbo].[CatalogC].[objId], [SkyNode_Test].[dbo].[CatalogC].[ra], [SkyNode_Test].[dbo].[CatalogC].[dec], [SkyNode_Test].[dbo].[CatalogC].[astroErr], [SkyNode_Test].[dbo].[CatalogC].[cx], [SkyNode_Test].[dbo].[CatalogC].[cy], [SkyNode_Test].[dbo].[CatalogC].[cz], [SkyNode_Test].[dbo].[CatalogC].[htmId], [SkyNode_Test].[dbo].[CatalogC].[zoneId], [SkyNode_Test].[dbo].[CatalogC].[mag_1], [SkyNode_Test].[dbo].[CatalogC].[mag_2], [SkyNode_Test].[dbo].[CatalogC].[mag_3] FROM [SkyNode_Test].[dbo].[CatalogC]) [c]
WHERE [c].[ra] BETWEEN 1 AND 2";

            var res = GetExecuteQueryTextTestHelper(sql);

            Assert.AreEqual(gt, res);

        }

        #endregion

    }
}
