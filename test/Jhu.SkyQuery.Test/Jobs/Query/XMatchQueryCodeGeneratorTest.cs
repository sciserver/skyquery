using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Schema.SqlServer;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Jobs.Query;
using Jhu.SkyQuery.Jobs.Query;
using System.Reflection;

namespace Jhu.SkyQuery.Jobs.Query.Test
{
    [TestClass]
    public class XMatchQueryCodeGeneratorTest : RegionQueryCodeGeneratorTest
    {
        private XMatchQueryCodeGenerator CodeGenerator
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

        private string[] GeneratePropagatedColumnListTestHelper(string sql, ColumnListInclude include)
        {
            var res = new List<string>();
            var q = CreateQuery(sql);
            var cg = new XMatchQueryCodeGenerator(q);

            foreach (var qs in q.SelectStatement.EnumerateQuerySpecifications())
            {
                foreach (var ts in qs.EnumerateSourceTables(false))
                {
                    var type = ColumnListType.ForSelectWithEscapedNameNoAlias;
                    var nullType = ColumnListNullType.Nothing;
                    var leadingComma = false;

                    res.Add(cg.GeneratePropagatedColumnList(ts, "tablealias", include, type, nullType, leadingComma));
                }
            }

            return res.ToArray();
        }

        #region Propagated columns tests

        [TestMethod]
        public void GetPropagatedColumnListTest()
        {
            var sql =
@"SELECT a.objID, a.ra, a.dec,
         b.objID, b.ra, b.dec,
         x.ra, x.dec
FROM XMATCH
    (MUST EXIST IN TEST:CatalogA a WITH(POINT(a.cx, a.cy, a.cz)),
     MUST EXIST IN TEST:CatalogB b WITH(POINT(b.cx, b.cy, b.cz)),
     LIMIT BAYESFACTOR TO 1000) AS x";

            var res = GeneratePropagatedColumnListTestHelper(sql, ColumnListInclude.Referenced);
            Assert.AreEqual("[tablealias].[_TEST_dbo_CatalogA_a_objId], [tablealias].[_TEST_dbo_CatalogA_a_ra], [tablealias].[_TEST_dbo_CatalogA_a_dec], [tablealias].[_TEST_dbo_CatalogA_a_cx], [tablealias].[_TEST_dbo_CatalogA_a_cy], [tablealias].[_TEST_dbo_CatalogA_a_cz]", res[1]);

            res = GeneratePropagatedColumnListTestHelper(sql, ColumnListInclude.PrimaryKey);
            Assert.AreEqual("[tablealias].[_TEST_dbo_CatalogA_a_objId]", res[1]);

            res = GeneratePropagatedColumnListTestHelper(sql, ColumnListInclude.All);
            Assert.AreEqual("[tablealias].[_TEST_dbo_CatalogA_a_objId], [tablealias].[_TEST_dbo_CatalogA_a_ra], [tablealias].[_TEST_dbo_CatalogA_a_dec], [tablealias].[_TEST_dbo_CatalogA_a_cx], [tablealias].[_TEST_dbo_CatalogA_a_cy], [tablealias].[_TEST_dbo_CatalogA_a_cz]", res[1]);
        }

        [TestMethod]
        public void GetPropagatedColumnListTest2()
        {
            var sql =
@"SELECT a.ra
FROM XMATCH
    (MUST EXIST IN TEST:CatalogA a WITH(POINT(a.ra, a.dec)),
     MUST EXIST IN TEST:CatalogB b WITH(POINT(b.cx, b.cy, b.cz)),
     LIMIT BAYESFACTOR TO 1e3) AS x";

            var res = GeneratePropagatedColumnListTestHelper(sql, ColumnListInclude.Referenced);
            Assert.AreEqual("[tablealias].[_TEST_dbo_CatalogA_a_ra], [tablealias].[_TEST_dbo_CatalogA_a_dec]", res[1]);

            res = GeneratePropagatedColumnListTestHelper(sql, ColumnListInclude.PrimaryKey);
            Assert.AreEqual("[tablealias].[_TEST_dbo_CatalogA_a_objId]", res[1]);

            res = GeneratePropagatedColumnListTestHelper(sql, ColumnListInclude.All);
            Assert.AreEqual("[tablealias].[_TEST_dbo_CatalogA_a_objId], [tablealias].[_TEST_dbo_CatalogA_a_ra], [tablealias].[_TEST_dbo_CatalogA_a_dec]", res[1]);
        }

        [TestMethod]
        public void GetPropagatedColumnListWithAliasesTest()
        {
            var sql =
@"SELECT a.ra
FROM XMATCH
    (MUST EXIST IN TEST:CatalogA a WITH(POINT(a.ra, a.dec)),
     MUST EXIST IN TEST:CatalogB b WITH(POINT(b.cx, b.cy, b.cz)),
     LIMIT BAYESFACTOR TO 1e3) AS x";

            var res = GeneratePropagatedColumnListTestHelper(sql, ColumnListInclude.Referenced);
            Assert.AreEqual("[tablealias].[_TEST_dbo_CatalogA_a_ra], [tablealias].[_TEST_dbo_CatalogA_a_dec]", res[1]);

            res = GeneratePropagatedColumnListTestHelper(sql, ColumnListInclude.PrimaryKey);
            Assert.AreEqual("[tablealias].[_TEST_dbo_CatalogA_a_objId]", res[1]);

            res = GeneratePropagatedColumnListTestHelper(sql, ColumnListInclude.All);
            Assert.AreEqual("[tablealias].[_TEST_dbo_CatalogA_a_objId], [tablealias].[_TEST_dbo_CatalogA_a_ra], [tablealias].[_TEST_dbo_CatalogA_a_dec]", res[1]);
        }

        #endregion


        // ---
        /* TODO: delete
        private string GetExecuteQueryTextTestHelper(string query)
        {
            var sm = CreateSchemaManager();

            var xmq = new BayesFactorXMatchQuery(null);
            xmq.QueryString = query;
            xmq.QueryFactoryTypeName = Jhu.Graywulf.Util.TypeNameFormatter.ToUnversionedAssemblyQualifiedName(typeof(Jhu.SkyQuery.Jobs.Query.XMatchQueryFactory));
            xmq.DefaultDataset = (SqlServerDataset)sm.Datasets[Jhu.Graywulf.Test.Constants.TestDatasetName];
            xmq.TemporaryDataset = (SqlServerDataset)sm.Datasets[Jhu.Graywulf.Test.Constants.TestDatasetName];
            xmq.CodeDataset = (SqlServerDataset)sm.Datasets[Jhu.Graywulf.Test.Constants.TestDatasetName];

            var xmqp = new BayesFactorXMatchQueryPartition(xmq);
            xmqp.ID = 0;
            xmqp.InitializeQueryObject(null);

            var qs = xmqp.Query.SelectStatement.EnumerateQuerySpecifications().First();
            var fc = qs.FindDescendant<Jhu.Graywulf.SqlParser.FromClause>();
            var xmtables = qs.FindDescendantRecursive<Jhu.SkyQuery.Parser.XMatchTableSource>().EnumerateXMatchTableSpecifications().ToArray();
            xmqp.GenerateSteps(xmtables);

            var xmtstr = new List<TableReference>(xmtables.Select(ts => ts.TableReference));

            var m = xmqp.GetType().GetMethod("GetExecuteQueryText", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(m);

            return (string)m.Invoke(xmqp, null);
        }
         * */

        private string GetExecuteQueryTextTestHelper(string sql)
        {
            var xmq = CreateQuery(sql);
            xmq.ExecutionMode = ExecutionMode.SingleServer;

            var xmqp = new BayesFactorXMatchQueryPartition((BayesFactorXMatchQuery)xmq);
            xmqp.ID = 0;
            xmqp.InitializeQueryObject(null);

            var qs = xmqp.Query.SelectStatement.EnumerateQuerySpecifications().First();
            var fc = qs.FindDescendant<Jhu.Graywulf.SqlParser.FromClause>();
            var xmtables = qs.FindDescendantRecursive<Jhu.SkyQuery.Parser.XMatchTableSource>().EnumerateXMatchTableSpecifications().ToArray();
            xmqp.GenerateSteps(xmtables);

            var cg = new XMatchQueryCodeGenerator(xmqp);
            var res = cg.GetExecuteQuery(xmq.SelectStatement);

            return res.Query;
        }

        [TestMethod]
        public void GetOutputSelectQueryTest()
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
@"SELECT [matchtable].[_TEST_dbo_CatalogA_a_objId] AS [a_objId], [matchtable].[_TEST_dbo_CatalogA_a_ra] AS [a_ra], [matchtable].[_TEST_dbo_CatalogA_a_dec] AS [a_dec],
[matchtable].[_TEST_dbo_CatalogB_b_objId] AS [b_objId], [matchtable].[_TEST_dbo_CatalogB_b_ra] AS [b_ra], [matchtable].[_TEST_dbo_CatalogB_b_dec] AS [b_dec],
[matchtable].[RA] AS [x_RA], [matchtable].[Dec] AS [x_Dec]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_1] AS [matchtable]
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
     (MUST EXIST IN TEST:CatalogA a WITH(POINT(a.cx, a.cy, a.cz)),
      MUST EXIST IN TEST:CatalogB b WITH(POINT(b.cx, b.cy, b.cz)),
      MUST EXIST IN TEST:CatalogC c WITH(POINT(c.cx, c.cy, c.cz)),
      LIMIT BAYESFACTOR TO 1e3) AS x";

            var gt =
@"SELECT [matchtable].[_TEST_dbo_CatalogA_a_objId] AS [a_objId], [matchtable].[_TEST_dbo_CatalogA_a_ra] AS [a_ra], [matchtable].[_TEST_dbo_CatalogA_a_dec] AS [a_dec],
         [matchtable].[_TEST_dbo_CatalogB_b_objId] AS [b_objId], [matchtable].[_TEST_dbo_CatalogB_b_ra] AS [b_ra], [matchtable].[_TEST_dbo_CatalogB_b_dec] AS [b_dec],
         [matchtable].[RA] AS [x_RA], [matchtable].[Dec] AS [x_Dec]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_2] AS [matchtable]
";

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
     (MUST EXIST IN TEST:CatalogA a WITH(POINT(a.cx, a.cy, a.cz)),
      MUST EXIST IN TEST:CatalogB b WITH(POINT(b.cx, b.cy, b.cz)),
      LIMIT BAYESFACTOR TO 1e3) AS x
CROSS JOIN TEST:CatalogC c";

            var gt =
@"SELECT [matchtable].[_TEST_dbo_CatalogA_a_objId] AS [a_objId], [matchtable].[_TEST_dbo_CatalogA_a_ra] AS [a_ra], [matchtable].[_TEST_dbo_CatalogA_a_dec] AS [a_dec],
         [matchtable].[_TEST_dbo_CatalogB_b_objId] AS [b_objId], [matchtable].[_TEST_dbo_CatalogB_b_ra] AS [b_ra], [matchtable].[_TEST_dbo_CatalogB_b_dec] AS [b_dec],
         [c].[objId] AS [c_objId], [c].[ra] AS [c_ra], [c].[dec] AS [c_dec],
         [matchtable].[RA] AS [x_RA], [matchtable].[Dec] AS [x_Dec]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_1] AS [matchtable]
CROSS JOIN [SkyNode_Test].[dbo].[CatalogC] [c]
";

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
     (MUST EXIST IN TEST:CatalogA a WITH(POINT(a.cx, a.cy, a.cz)),
      MUST EXIST IN TEST:CatalogB b WITH(POINT(b.cx, b.cy, b.cz)),
      LIMIT BAYESFACTOR TO 1e3) AS x
INNER JOIN TEST:CatalogC c ON c.objId = a.objId";

            var gt =
@"SELECT [matchtable].[_TEST_dbo_CatalogA_a_objId] AS [a_objId], [matchtable].[_TEST_dbo_CatalogA_a_ra] AS [a_ra], [matchtable].[_TEST_dbo_CatalogA_a_dec] AS [a_dec],
         [matchtable].[_TEST_dbo_CatalogB_b_objId] AS [b_objId], [matchtable].[_TEST_dbo_CatalogB_b_ra] AS [b_ra], [matchtable].[_TEST_dbo_CatalogB_b_dec] AS [b_dec],
         [c].[objId] AS [c_objId], [c].[ra] AS [c_ra], [c].[dec] AS [c_dec],
         [matchtable].[RA] AS [x_RA], [matchtable].[Dec] AS [x_Dec]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_1] AS [matchtable]
INNER JOIN [SkyNode_Test].[dbo].[CatalogC] [c] ON [c].[objId] = [matchtable].[_TEST_dbo_CatalogA_a_objId]
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
     (MUST EXIST IN TEST:CatalogA a WITH(POINT(a.cx, a.cy, a.cz)),
      MUST EXIST IN TEST:CatalogB b WITH(POINT(b.cx, b.cy, b.cz)),
      LIMIT BAYESFACTOR TO 1e3) AS x
WHERE a.ra BETWEEN 1 AND 2";

            var gt =
@"SELECT [matchtable].[_TEST_dbo_CatalogA_a_objId] AS [a_objId], [matchtable].[_TEST_dbo_CatalogB_b_objId] AS [b_objId]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_1] AS [matchtable]
WHERE [matchtable].[_TEST_dbo_CatalogA_a_ra] BETWEEN 1 AND 2
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
     (MUST EXIST IN TEST:CatalogA a WITH(POINT(a.cx, a.cy, a.cz)),
      MUST EXIST IN TEST:CatalogB b WITH(POINT(b.cx, b.cy, b.cz)),
      LIMIT BAYESFACTOR TO 1e3) AS x,
     TEST:CatalogC c
WHERE c.ra BETWEEN 1 AND 2";

            var gt =
@"SELECT [matchtable].[_TEST_dbo_CatalogA_a_objId] AS [a_objId], [matchtable].[_TEST_dbo_CatalogB_b_objId] AS [b_objId]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_1] AS [matchtable],
     [SkyNode_Test].[dbo].[CatalogC] [c]
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
     (MUST EXIST IN TEST:CatalogA a WITH(POINT(a.cx, a.cy, a.cz)),
      MUST EXIST IN TEST:CatalogB b WITH(POINT(b.cx, b.cy, b.cz)),
      LIMIT BAYESFACTOR TO 1e3) AS x,
     (SELECT * FROM TEST:CatalogC) c
WHERE c.ra BETWEEN 1 AND 2";

            var gt =
@"SELECT [matchtable].[_TEST_dbo_CatalogA_a_objId] AS [a_objId], [matchtable].[_TEST_dbo_CatalogB_b_objId] AS [b_objId], [c].[objId] AS [c_objId]
FROM [Graywulf_Temp].[dbo].[skyquerytemp_0_Match_1] AS [matchtable],
     (SELECT [SkyNode_Test].[dbo].[CatalogC].[objId], [SkyNode_Test].[dbo].[CatalogC].[ra], [SkyNode_Test].[dbo].[CatalogC].[dec], [SkyNode_Test].[dbo].[CatalogC].[astroErr], [SkyNode_Test].[dbo].[CatalogC].[cx], [SkyNode_Test].[dbo].[CatalogC].[cy], [SkyNode_Test].[dbo].[CatalogC].[cz], [SkyNode_Test].[dbo].[CatalogC].[htmId], [SkyNode_Test].[dbo].[CatalogC].[zoneId], [SkyNode_Test].[dbo].[CatalogC].[mag_1], [SkyNode_Test].[dbo].[CatalogC].[mag_2], [SkyNode_Test].[dbo].[CatalogC].[mag_3] FROM [SkyNode_Test].[dbo].[CatalogC]) [c]
WHERE [c].[ra] BETWEEN 1 AND 2
";

            var res = GetExecuteQueryTextTestHelper(sql);

            Assert.AreEqual(gt, res);

        }

    }
}
