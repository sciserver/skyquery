using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Parsing;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.Graywulf.Sql.Schema;
using Jhu.Graywulf.Sql.Schema.SqlServer;
using Jhu.Graywulf.Sql.NameResolution;
using Jhu.Graywulf.Sql.CodeGeneration;
using Jhu.Graywulf.Sql.CodeGeneration.SqlServer;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.SkyQuery.Sql.Parsing;

namespace Jhu.SkyQuery.Sql.Jobs.Query
{
    [TestClass]
    public class XMatchQueryCodeGeneratorTest : SkyQueryTestBase
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

        protected XMatchQueryCodeGenerator CodeGenerator
        {
            get
            {
                return new XMatchQueryCodeGenerator()
                {
                    CodeDataset = new Jhu.Graywulf.Sql.Schema.SqlServer.SqlServerDataset()
                    {
                        Name = "CODE",
                        ConnectionString = "data source=localhost;initial catalog=SkyQuery_Code",
                    }
                };
            }
        }

        protected XMatchSelectStatement Parse(string sql)
        {
            var p = new SkyQueryParser();
            var script = p.Execute<SkyQuery.Sql.Parsing.StatementBlock>(sql);
            var ss = script.FindDescendantRecursive<XMatchSelectStatement>();
            var qs = (XMatchQuerySpecification)ss.QueryExpression.EnumerateQuerySpecifications().First();

            var nr = new SkyQueryNameResolver();
            nr.DefaultTableDatasetName = Jhu.Graywulf.Test.Constants.TestDatasetName;
            nr.DefaultFunctionDatasetName = Jhu.Graywulf.Test.Constants.CodeDatasetName;
            nr.DefaultDataTypeDatasetName = Jhu.Graywulf.Test.Constants.CodeDatasetName;
            nr.SchemaManager = CreateSchemaManager();
            nr.Execute(script);

            return ss;
        }

        #region Helper functions

        private string[] GeneratePropagatedColumnListTestHelper(string sql, ColumnContext context)
        {
            var res = new List<string>();
            var q = CreateQuery(sql);
            var ss = q.QueryDetails.ParsingTree.FindDescendantRecursive<SelectStatement>();
            var cg = new XMatchQueryCodeGenerator(q);

            foreach (var qs in ss.QueryExpression.EnumerateQuerySpecifications())
            {
                foreach (var ts in qs.EnumerateSourceTables(false))
                {
                    var columns = new SqlServerColumnListGenerator(ts.TableReference.FilterColumnReferences(context))
                    {
                        ListType = ColumnListType.SelectWithEscapedNameNoAlias,
                        TableAlias = "tablealias",
                    };

                    res.Add(columns.Execute());
                }
            }

            return res.ToArray();
        }

        private string GetExecuteQueryTextTestHelper(string sql)
        {
            using (var context = ContextManager.Instance.CreateReadOnlyContext())
            {
                var sm = new GraywulfSchemaManager(new FederationContext(context, null));

                var xmq = CreateQuery(sql);
                xmq.Parameters.ExecutionMode = ExecutionMode.SingleServer;

                var xmqp = new BayesFactorXMatchQueryPartition((BayesFactorXMatchQuery)xmq);
                xmqp.ID = 0;
                xmqp.InitializeQueryObject(null);
                xmqp.Parameters.DefaultSourceDataset =
                    xmqp.Parameters.DefaultOutputDataset =
                    (SqlServerDataset)sm.Datasets[Jhu.Graywulf.Test.Constants.TestDatasetName];
                xmqp.CodeDataset = (SqlServerDataset)sm.Datasets[Jhu.Graywulf.Registry.Constants.CodeDbName];

                // TODO: where to take this? let's just hack it for now
                xmqp.TemporaryDataset = new SqlServerDataset()
                {
                    Name = Jhu.Graywulf.Registry.Constants.TempDbName,
                    DatabaseName = "Graywulf_Temp"
                };

                var qs = xmqp.Query.QueryDetails.ParsingTree.FindDescendantRecursive<XMatchSelectStatement>().QueryExpression.EnumerateQuerySpecifications().First();
                var fc = qs.FindDescendant<Jhu.Graywulf.Sql.Parsing.FromClause>();
                var xmtables = qs.FindDescendantRecursive<Jhu.SkyQuery.Sql.Parsing.XMatchTableSource>().EnumerateXMatchTableSpecifications().ToArray();
                xmqp.GenerateSteps(xmtables);

                var cg = new XMatchQueryCodeGenerator(xmqp)
                {
                    ColumnNameRendering = NameRendering.FullyQualified,
                    ColumnAliasRendering = AliasRendering.Always,
                    DataTypeNameRendering = NameRendering.FullyQualified,
                    TableNameRendering = NameRendering.FullyQualified,
                    FunctionNameRendering = NameRendering.FullyQualified,
                };

                var res = cg.GetExecuteQuery();

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
            Assert.AreEqual("[tablealias].[__a_objId], [tablealias].[__a_ra], [tablealias].[__a_dec]", res[1]);

            res = GeneratePropagatedColumnListTestHelper(sql, ColumnContext.PrimaryKey);
            Assert.AreEqual("[tablealias].[__a_objId]", res[1]);

            res = GeneratePropagatedColumnListTestHelper(sql, ColumnContext.AllReferenced);
            Assert.AreEqual("[tablealias].[__a_objId], [tablealias].[__a_ra], [tablealias].[__a_dec], [tablealias].[__a_cx], [tablealias].[__a_cy], [tablealias].[__a_cz]", res[1]);
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
            Assert.AreEqual("[tablealias].[__a_ra]", res[1]);

            res = GeneratePropagatedColumnListTestHelper(sql, ColumnContext.PrimaryKey);
            Assert.AreEqual("[tablealias].[__a_objId]", res[1]);

            res = GeneratePropagatedColumnListTestHelper(sql, ColumnContext.AllReferenced);
            Assert.AreEqual("[tablealias].[__a_objId], [tablealias].[__a_ra], [tablealias].[__a_dec]", res[1]);
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
            Assert.AreEqual("[tablealias].[__a_ra]", res[1]);

            res = GeneratePropagatedColumnListTestHelper(sql, ColumnContext.PrimaryKey);
            Assert.AreEqual("[tablealias].[__a_objId]", res[1]);

            res = GeneratePropagatedColumnListTestHelper(sql, ColumnContext.AllReferenced);
            Assert.AreEqual("[tablealias].[__a_objId], [tablealias].[__a_ra], [tablealias].[__a_dec]", res[1]);
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
            var tts = qs.QueryExpression.EnumerateSourceTables(false).ToArray();

            var coords = new TableCoordinates((SkyQuery.Sql.Parsing.CoordinatesTableSource)tts[1]);
            Assert.IsNotNull(CodeGenerator.FindHtmIndex(coords));

            coords = new TableCoordinates((SkyQuery.Sql.Parsing.CoordinatesTableSource)tts[2]);
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
            var tts = qs.QueryExpression.EnumerateSourceTables(false).ToArray();

            var coords = new TableCoordinates((SkyQuery.Sql.Parsing.CoordinatesTableSource)tts[1]);
            Assert.IsNull(CodeGenerator.FindHtmIndex(coords));

            coords = new TableCoordinates((SkyQuery.Sql.Parsing.CoordinatesTableSource)tts[2]);
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
            var tts = qs.QueryExpression.EnumerateSourceTables(false).ToArray();

            var coords = new TableCoordinates((SkyQuery.Sql.Parsing.CoordinatesTableSource)tts[1]);
            Assert.IsNotNull(CodeGenerator.FindZoneIndex(coords));

            coords = new TableCoordinates((SkyQuery.Sql.Parsing.CoordinatesTableSource)tts[2]);
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
            var tts = qs.QueryExpression.EnumerateSourceTables(false).ToArray();

            var coords = new TableCoordinates((SkyQuery.Sql.Parsing.CoordinatesTableSource)tts[1]);
            Assert.IsNull(CodeGenerator.FindZoneIndex(coords));

            coords = new TableCoordinates((SkyQuery.Sql.Parsing.CoordinatesTableSource)tts[2]);
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
@"SELECT [__match].[__a_objId] AS [a_objId], [__match].[__a_ra] AS [a_ra], [__match].[__a_dec] AS [a_dec],
[__match].[__b_objId] AS [b_objId], [__match].[__b_ra] AS [b_ra], [__match].[__b_dec] AS [b_dec],
[__match].[RA] AS [x_RA], [__match].[Dec] AS [x_Dec]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_1] AS [__match]
";

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
@"SELECT [__match].[__a_objId] AS [a_objId], [__match].[__a_ra] AS [a_ra], [__match].[__a_dec] AS [a_dec],
[__match].[__b_objId] AS [b_objId], [__match].[__b_ra] AS [b_ra], [__match].[__b_dec] AS [b_dec],
[__match].[RA] AS [x_RA], [__match].[Dec] AS [x_Dec]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_1] AS [__match]
";

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
@"SELECT [__match].[__a_objId] AS [a_objId], [__match].[__a_ra] AS [a_ra], [__match].[__a_dec] AS [a_dec],
         [__match].[__b_objId] AS [b_objId], [__match].[__b_ra] AS [b_ra], [__match].[__b_dec] AS [b_dec],
         [__match].[RA] AS [x_RA], [__match].[Dec] AS [x_Dec]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_2] AS [__match]
";

            var res = GetExecuteQueryTextTestHelper(sql);

            Assert.AreEqual(gt, res);
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
@"SELECT [__match].[__a_objId] AS [a_objId], [__match].[__a_ra] AS [a_ra], [__match].[__a_dec] AS [a_dec],
         [__match].[__b_objId] AS [b_objId], [__match].[__b_ra] AS [b_ra], [__match].[__b_dec] AS [b_dec],
         [c].[objId] AS [c_objId], [c].[ra] AS [c_ra], [c].[dec] AS [c_dec],
         [__match].[RA] AS [x_RA], [__match].[Dec] AS [x_Dec]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_1] AS [__match]
CROSS JOIN [SkyNode_TEST].[dbo].[CatalogC] [c]
";

            var res = GetExecuteQueryTextTestHelper(sql);

            Assert.AreEqual(gt, res);
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
@"SELECT [__match].[__a_objId] AS [a_objId], [__match].[__a_ra] AS [a_ra], [__match].[__a_dec] AS [a_dec],
         [__match].[__b_objId] AS [b_objId], [__match].[__b_ra] AS [b_ra], [__match].[__b_dec] AS [b_dec],
         [c].[objId] AS [c_objId], [c].[ra] AS [c_ra], [c].[dec] AS [c_dec],
         [__match].[RA] AS [x_RA], [__match].[Dec] AS [x_Dec]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_1] AS [__match]
INNER JOIN [SkyNode_TEST].[dbo].[CatalogC] [c] ON [c].[objId] = [__match].[__a_objId]
";

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
@"SELECT [__match].[__a_objId] AS [a_objId], [__match].[__b_objId] AS [b_objId]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_1] AS [__match]
WHERE [__match].[__a_ra] BETWEEN 1 AND 2
";

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
@"SELECT [__match].[__a_objId] AS [a_objId], [__match].[__b_objId] AS [b_objId]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_1] AS [__match],
     [SkyNode_TEST].[dbo].[CatalogC] [c]
WHERE [c].[ra] BETWEEN 1 AND 2
";

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
@"SELECT [__match].[__a_objId] AS [a_objId], [__match].[__b_objId] AS [b_objId], [c].[objId] AS [c_objId]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_1] AS [__match],
     (SELECT [SkyNode_TEST].[dbo].[CatalogC].[objId], [SkyNode_TEST].[dbo].[CatalogC].[ra], [SkyNode_TEST].[dbo].[CatalogC].[dec], [SkyNode_TEST].[dbo].[CatalogC].[astroErr], [SkyNode_TEST].[dbo].[CatalogC].[cx], [SkyNode_TEST].[dbo].[CatalogC].[cy], [SkyNode_TEST].[dbo].[CatalogC].[cz], [SkyNode_TEST].[dbo].[CatalogC].[htmId], [SkyNode_TEST].[dbo].[CatalogC].[zoneId], [SkyNode_TEST].[dbo].[CatalogC].[mag_1], [SkyNode_TEST].[dbo].[CatalogC].[mag_2], [SkyNode_TEST].[dbo].[CatalogC].[mag_3] FROM [SkyNode_TEST].[dbo].[CatalogC]) [c]
WHERE [c].[ra] BETWEEN 1 AND 2
";

            var res = GetExecuteQueryTextTestHelper(sql);

            Assert.AreEqual(gt, res);

        }

        #endregion

    }
}
