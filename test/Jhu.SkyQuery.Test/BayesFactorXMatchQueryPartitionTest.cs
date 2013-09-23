using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Schema.SqlServer;
using Jhu.Graywulf.Registry;
using Jhu.SkyQuery.Jobs.Query;
using System.Reflection;

namespace Jhu.SkyQuery.Test
{
    [TestClass]
    public class BayesFactorXMatchQueryPartitionTest
    {
        private SchemaManager CreateSchemaManager()
        {
            var sm = new SqlServerSchemaManager();
            var ds = new SqlServerDataset("Test", "Data Source=localhost;Initial Catalog=SkyNode_Test;Integrated Security=true");

            sm.Datasets[ds.Name] = ds;

            return sm;
        }

        private QuerySpecification Parse(string query)
        {
            var p = new Jhu.SkyQuery.Parser.SkyQueryParser();
            var ss = (SelectStatement)p.Execute(query);

            var nr = new Jhu.SkyQuery.Parser.SkyQueryNameResolver();
            nr.DefaultTableDatasetName = "Test";
            nr.DefaultTableSchemaName = "dbo";
            nr.DefaultFunctionDatasetName = "Code";
            nr.DefaultFunctionSchemaName = "dbo";
            nr.SchemaManager = CreateSchemaManager();
            nr.Execute(ss);

            return (QuerySpecification)ss.EnumerateQuerySpecifications().First(); ;
        }

        // --- GetPropagatedColumnList

        private string GetPropagatedColumnListTestHelper(string query, XMatchQueryPartition.ColumnListInclude include)
        {
            var sm = CreateSchemaManager();

            var xmq = new BayesFactorXMatchQuery(null);
            xmq.QueryString = query;
            xmq.QueryFactoryTypeName = typeof(Jhu.SkyQuery.Jobs.Query.XMatchQueryFactory).AssemblyQualifiedName;
            xmq.DefaultDataset = (SqlServerDataset)sm.Datasets["Test"];
            xmq.TemporaryDataset = (SqlServerDataset)sm.Datasets["Test"];

            var xmqp = new BayesFactorXMatchQueryPartition(xmq, null);
            xmqp.ID = 0;
            xmqp.InitializeQueryObject(null);

            var qs = xmqp.SelectStatement.EnumerateQuerySpecifications().First();
            var fc = qs.FindDescendant<Jhu.Graywulf.SqlParser.FromClause>();
            var xmc = qs.FindDescendant<Jhu.SkyQuery.Parser.XMatchClause>();

            var xmtables = xmc.EnumerateXMatchTableSpecifications().ToArray();
            xmqp.GenerateSteps(xmtables);

            var xmtstr = new List<TableReference>(xmtables.Select(ts => ts.TableReference));

            var m = xmqp.GetType().GetMethod("GetPropagatedColumnList", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(m);

            return (string)m.Invoke(xmqp, new object[] { xmtables[0], XMatchQueryPartition.ColumnListType.ForSelectNoAlias, include, XMatchQueryPartition.ColumnListNullType.Nothing, "tablealias" });
        }

        [TestMethod]
        public void GetPropagatedColumnListTest()
        {
            var sql =
@"SELECT a.objID, a.ra, a.dec,
         b.objID, b.ra, b.dec,
         x.ra, x.dec
FROM CatalogA a, CatalogB b
XMATCH BAYESFACTOR x
MUST EXIST a on POINT(a.cx, a.cy, a.cz)
MUST EXIST b on POINT(b.cx, b.cy, b.cz)
HAVING LIMIT 1e3";

            var res = GetPropagatedColumnListTestHelper(sql, XMatchQueryPartition.ColumnListInclude.Referenced);
            Assert.AreEqual("[tablealias].[_Test__CatalogA_a_objId], [tablealias].[_Test__CatalogA_a_ra], [tablealias].[_Test__CatalogA_a_dec], [tablealias].[_Test__CatalogA_a_cx], [tablealias].[_Test__CatalogA_a_cy], [tablealias].[_Test__CatalogA_a_cz]", res);

            res = GetPropagatedColumnListTestHelper(sql, XMatchQueryPartition.ColumnListInclude.PrimaryKey);
            Assert.AreEqual("[tablealias].[_Test__CatalogA_a_objId]", res);

            res = GetPropagatedColumnListTestHelper(sql, XMatchQueryPartition.ColumnListInclude.All);
            Assert.AreEqual("[tablealias].[_Test__CatalogA_a_objId], [tablealias].[_Test__CatalogA_a_ra], [tablealias].[_Test__CatalogA_a_dec], [tablealias].[_Test__CatalogA_a_cx], [tablealias].[_Test__CatalogA_a_cy], [tablealias].[_Test__CatalogA_a_cz]", res);
        }

        [TestMethod]
        public void GetPropagatedColumnListTest2()
        {
            var sql =
@"SELECT a.ra
FROM CatalogA a, CatalogB b
XMATCH BAYESFACTOR x
MUST EXIST a on POINT(a.ra, a.dec)
MUST EXIST b on POINT(b.cx, b.cy, b.cz)
HAVING LIMIT 1e3";

            var res = GetPropagatedColumnListTestHelper(sql, XMatchQueryPartition.ColumnListInclude.Referenced);
            Assert.AreEqual("[tablealias].[_Test__CatalogA_a_ra], [tablealias].[_Test__CatalogA_a_dec]", res);

            res = GetPropagatedColumnListTestHelper(sql, XMatchQueryPartition.ColumnListInclude.PrimaryKey);
            Assert.AreEqual("[tablealias].[_Test__CatalogA_a_objId]", res);

            res = GetPropagatedColumnListTestHelper(sql, XMatchQueryPartition.ColumnListInclude.All);
            Assert.AreEqual("[tablealias].[_Test__CatalogA_a_objId], [tablealias].[_Test__CatalogA_a_ra], [tablealias].[_Test__CatalogA_a_dec]", res);
        }

        [TestMethod]
        public void GetPropagatedColumnListWithAliasesTest()
        {
            var sql =
@"SELECT a.ra colalias
FROM CatalogA a, CatalogB b
XMATCH BAYESFACTOR x
MUST EXIST a on POINT(a.ra, a.dec)
MUST EXIST b on POINT(b.cx, b.cy, b.cz)
HAVING LIMIT 1e3";

            var res = GetPropagatedColumnListTestHelper(sql, XMatchQueryPartition.ColumnListInclude.Referenced);
            Assert.AreEqual("[tablealias].[_Test__CatalogA_a_ra], [tablealias].[_Test__CatalogA_a_dec]", res);

            res = GetPropagatedColumnListTestHelper(sql, XMatchQueryPartition.ColumnListInclude.PrimaryKey);
            Assert.AreEqual("[tablealias].[_Test__CatalogA_a_objId]", res);

            res = GetPropagatedColumnListTestHelper(sql, XMatchQueryPartition.ColumnListInclude.All);
            Assert.AreEqual("[tablealias].[_Test__CatalogA_a_objId], [tablealias].[_Test__CatalogA_a_ra], [tablealias].[_Test__CatalogA_a_dec]", res);
        }

        // ---

        private string GetOutputSelectQueryTestHelper(string query)
        {
            var sm = CreateSchemaManager();

            var xmq = new BayesFactorXMatchQuery(null);
            xmq.QueryString = query;
            xmq.QueryFactoryTypeName = typeof(Jhu.SkyQuery.Jobs.Query.XMatchQueryFactory).AssemblyQualifiedName;
            xmq.DefaultDataset = (SqlServerDataset)sm.Datasets["Test"];
            xmq.TemporaryDataset = (SqlServerDataset)sm.Datasets["Test"];

            var xmqp = new BayesFactorXMatchQueryPartition(xmq, null);
            xmqp.ID = 0;
            xmqp.InitializeQueryObject(null);

            var qs = xmqp.SelectStatement.EnumerateQuerySpecifications().First();
            var fc = qs.FindDescendant<Jhu.Graywulf.SqlParser.FromClause>();
            var xmc = qs.FindDescendant<Jhu.SkyQuery.Parser.XMatchClause>();

            var xmtables = xmc.EnumerateXMatchTableSpecifications().ToArray();
            xmqp.GenerateSteps(xmtables);

            var xmtstr = new List<TableReference>(xmtables.Select(ts => ts.TableReference));

            var m = xmqp.GetType().GetMethod("GetOutputSelectQuery", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(m);

            return (string)m.Invoke(xmqp, null);
        }

        [TestMethod]
        public void GetOutputSelectQueryTest()
        {
            var sql =
@"SELECT a.objID, a.ra, a.dec,
b.objID, b.ra, b.dec,
x.ra, x.dec
FROM CatalogA a, CatalogB b
XMATCH BAYESFACTOR x
MUST EXIST a on POINT(a.cx, a.cy, a.cz)
MUST EXIST b on POINT(b.cx, b.cy, b.cz)
HAVING LIMIT 1e3";

            var res = GetOutputSelectQueryTestHelper(sql);

            Assert.AreEqual(
@"SELECT [matchtable].[_Test__CatalogA_a_objId] AS [a_objId], [matchtable].[_Test__CatalogA_a_ra] AS [a_ra], [matchtable].[_Test__CatalogA_a_dec] AS [a_dec],
[matchtable].[_Test__CatalogB_b_objId] AS [b_objId], [matchtable].[_Test__CatalogB_b_ra] AS [b_ra], [matchtable].[_Test__CatalogB_b_dec] AS [b_dec],
[matchtable].[RA] AS [x_RA], [matchtable].[Dec] AS [x_Dec]
FROM [SkyNode_Test].[dbo].[skyquerytemp_0_Match_1] AS [matchtable]
", res);

        }

        [TestMethod]
        public void GetOutputSelectQuery_ThreeTablesTest()
        {
            var sql =
@"SELECT a.objID, a.ra, a.dec,
         b.objID, b.ra, b.dec,
         x.ra, x.dec
FROM CatalogA a, CatalogB b CROSS JOIN CatalogC c
XMATCH BAYESFACTOR x
MUST EXIST a on POINT(a.cx, a.cy, a.cz)
MUST EXIST b on POINT(b.cx, b.cy, b.cz)
MUST EXIST c on POINT(c.cx, c.cy, c.cz)
HAVING LIMIT 1e3";

            Assert.AreEqual(
@"SELECT [matchtable].[_Test__CatalogA_a_objId] AS [a_objId], [matchtable].[_Test__CatalogA_a_ra] AS [a_ra], [matchtable].[_Test__CatalogA_a_dec] AS [a_dec],
         [matchtable].[_Test__CatalogB_b_objId] AS [b_objId], [matchtable].[_Test__CatalogB_b_ra] AS [b_ra], [matchtable].[_Test__CatalogB_b_dec] AS [b_dec],
         [matchtable].[RA] AS [x_RA], [matchtable].[Dec] AS [x_Dec]
FROM [SkyNode_Test].[dbo].[skyquerytemp_0_Match_2] AS [matchtable]
", GetOutputSelectQueryTestHelper(sql));
        }

        [TestMethod]
        public void GetOutputSelectQuery_CombinedJoinTest()
        {
            var sql =
@"SELECT a.objID, a.ra, a.dec,
         b.objID, b.ra, b.dec,
         c.objID, c.ra, c.dec,
         x.ra, x.dec
FROM CatalogA a, CatalogB b CROSS JOIN CatalogC c
XMATCH BAYESFACTOR x
MUST EXIST a on POINT(a.cx, a.cy, a.cz)
MUST EXIST b on POINT(b.cx, b.cy, b.cz)
HAVING LIMIT 1e3";

            Assert.AreEqual(
@"SELECT [matchtable].[_Test__CatalogA_a_objId] AS [a_objId], [matchtable].[_Test__CatalogA_a_ra] AS [a_ra], [matchtable].[_Test__CatalogA_a_dec] AS [a_dec],
         [matchtable].[_Test__CatalogB_b_objId] AS [b_objId], [matchtable].[_Test__CatalogB_b_ra] AS [b_ra], [matchtable].[_Test__CatalogB_b_dec] AS [b_dec],
         [c].[objId] AS [c_objId], [c].[ra] AS [c_ra], [c].[dec] AS [c_dec],
         [matchtable].[RA] AS [x_RA], [matchtable].[Dec] AS [x_Dec]
FROM [SkyNode_Test].[dbo].[skyquerytemp_0_Match_1] AS [matchtable] CROSS JOIN [SkyNode_Test].[dbo].[CatalogC] [c]
", GetOutputSelectQueryTestHelper(sql));
        }

        [TestMethod]
        public void GetOutputSelectQuery_InnerJoinTest()
        {
            var sql =
@"SELECT a.objID, a.ra, a.dec,
         b.objID, b.ra, b.dec,
         c.objID, c.ra, c.dec,
         x.ra, x.dec
FROM CatalogA a, CatalogB b INNER JOIN CatalogC c ON c.objId = a.objId
XMATCH BAYESFACTOR x
MUST EXIST a on POINT(a.cx, a.cy, a.cz)
MUST EXIST b on POINT(b.cx, b.cy, b.cz)
HAVING LIMIT 1e3";

            var res = GetOutputSelectQueryTestHelper(sql);

            Assert.AreEqual(
@"SELECT [matchtable].[_Test__CatalogA_a_objId] AS [a_objId], [matchtable].[_Test__CatalogA_a_ra] AS [a_ra], [matchtable].[_Test__CatalogA_a_dec] AS [a_dec],
         [matchtable].[_Test__CatalogB_b_objId] AS [b_objId], [matchtable].[_Test__CatalogB_b_ra] AS [b_ra], [matchtable].[_Test__CatalogB_b_dec] AS [b_dec],
         [c].[objId] AS [c_objId], [c].[ra] AS [c_ra], [c].[dec] AS [c_dec],
         [matchtable].[RA] AS [x_RA], [matchtable].[Dec] AS [x_Dec]
FROM [SkyNode_Test].[dbo].[skyquerytemp_0_Match_1] AS [matchtable] INNER JOIN [SkyNode_Test].[dbo].[CatalogC] [c] ON [c].[objId] = [matchtable].[_Test__CatalogA_a_objId]
", res);
        }

        [TestMethod]
        public void GetOutputSelectQuery_WhereTest()
        {
            var sql =
@"SELECT a.objID, b.objID
FROM CatalogA a, CatalogB b
XMATCH BAYESFACTOR x
MUST EXIST a on POINT(a.cx, a.cy, a.cz)
MUST EXIST b on POINT(b.cx, b.cy, b.cz)
HAVING LIMIT 1e3
WHERE a.ra BETWEEN 1 AND 2";

            var res = GetOutputSelectQueryTestHelper(sql);

            Assert.AreEqual(
@"SELECT [matchtable].[_Test__CatalogA_a_objId] AS [a_objId], [matchtable].[_Test__CatalogB_b_objId] AS [b_objId]
FROM [SkyNode_Test].[dbo].[skyquerytemp_0_Match_1] AS [matchtable]

WHERE [matchtable].[_Test__CatalogA_a_ra] BETWEEN 1 AND 2", res);
        }

        [TestMethod]
        public void GetOutputSelectQuery_WhereTest2()
        {
            var sql =
@"SELECT a.objID, b.objID
FROM CatalogA a, CatalogB b, CatalogC c
XMATCH BAYESFACTOR x
MUST EXIST a on POINT(a.cx, a.cy, a.cz)
MUST EXIST b on POINT(b.cx, b.cy, b.cz)
HAVING LIMIT 1e3
WHERE c.ra BETWEEN 1 AND 2";

            var res = GetOutputSelectQueryTestHelper(sql);

            Assert.AreEqual(
@"SELECT [matchtable].[_Test__CatalogA_a_objId] AS [a_objId], [matchtable].[_Test__CatalogB_b_objId] AS [b_objId]
FROM [SkyNode_Test].[dbo].[skyquerytemp_0_Match_1] AS [matchtable] , [SkyNode_Test].[dbo].[CatalogC] [c]

WHERE [c].[ra] BETWEEN 1 AND 2", res);
        }

        [TestMethod]
        public void GetOutputSelectQuery_SubqueryTest()
        {
            var sql =
@"SELECT a.objID, b.objID, c.objID
FROM CatalogA a, CatalogB b, (SELECT * FROM CatalogC) c
XMATCH BAYESFACTOR x
MUST EXIST a on POINT(a.cx, a.cy, a.cz)
MUST EXIST b on POINT(b.cx, b.cy, b.cz)
HAVING LIMIT 1e3
WHERE c.ra BETWEEN 1 AND 2";

            var res = GetOutputSelectQueryTestHelper(sql);

            Assert.AreEqual(
@"SELECT [matchtable].[_Test__CatalogA_a_objId] AS [a_objId], [matchtable].[_Test__CatalogB_b_objId] AS [b_objId], [c].[objId] AS [c_objId]
FROM [SkyNode_Test].[dbo].[skyquerytemp_0_Match_1] AS [matchtable] , (SELECT [SkyNode_Test].[dbo].[CatalogC].[objId], [SkyNode_Test].[dbo].[CatalogC].[ra], [SkyNode_Test].[dbo].[CatalogC].[dec], [SkyNode_Test].[dbo].[CatalogC].[astroErr], [SkyNode_Test].[dbo].[CatalogC].[cx], [SkyNode_Test].[dbo].[CatalogC].[cy], [SkyNode_Test].[dbo].[CatalogC].[cz], [SkyNode_Test].[dbo].[CatalogC].[htmId], [SkyNode_Test].[dbo].[CatalogC].[mag_1], [SkyNode_Test].[dbo].[CatalogC].[mag_2], [SkyNode_Test].[dbo].[CatalogC].[mag_3] FROM [SkyNode_Test].[dbo].[CatalogC]) [c]

WHERE [c].[ra] BETWEEN 1 AND 2", res);
        }
    }
}
