using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Schema.SqlServer;
using Jhu.SkyQuery.Jobs.Query;
using System.Reflection;

namespace Jhu.SkyQuery.Test
{
    [TestClass]
    public class XMatchQueryPartitionTest
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
            nr.SchemaManager = CreateSchemaManager();
            nr.Execute(ss);

            return (QuerySpecification)ss.EnumerateQuerySpecifications().First();
        }

        private string ReplaceXMatchTableSourcesTestHelper(string query)
        {
            var sm = CreateSchemaManager();

            var xmq = new BayesFactorXMatchQuery(null);
            xmq.QueryString = query;
            xmq.QueryFactoryTypeName = typeof(Jhu.SkyQuery.Jobs.Query.XMatchQueryFactory).AssemblyQualifiedName;
            xmq.DefaultDatasetName = "Test";
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

            var m = xmqp.GetType().GetMethod("SubstituteXMatchTableSources", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(m);

            m.Invoke(xmqp, new object[] { fc, xmtstr });

            return Jhu.Graywulf.SqlParser.SqlCodeGen.SqlServerCodeGenerator.GetCode(fc, true);
        }

        [TestMethod]
        public void ReplaceXMatchTableSourcesTest()
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

            Assert.AreEqual("FROM [SkyNode_Test].[dbo].[skyquerytemp_0_Match_1] AS [matchtable]", ReplaceXMatchTableSourcesTestHelper(sql));
        }

        [TestMethod]
        public void ReplaceXMatchTableSources_ThreeTablesTest()
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

            Assert.AreEqual("FROM [SkyNode_Test].[dbo].[skyquerytemp_0_Match_2] AS [matchtable]", ReplaceXMatchTableSourcesTestHelper(sql));
        }

        [TestMethod]
        public void ReplaceXMatchTableSources_CombinedJoinTest()
        {

            var sql =
@"SELECT a.objID, a.ra, a.dec,
         b.objID, b.ra, b.dec,
         x.ra, x.dec
FROM CatalogA a, CatalogB b CROSS JOIN CatalogC c
XMATCH BAYESFACTOR x
MUST EXIST a on POINT(a.cx, a.cy, a.cz)
MUST EXIST b on POINT(b.cx, b.cy, b.cz)
HAVING LIMIT 1e3";

            Assert.AreEqual("FROM [SkyNode_Test].[dbo].[skyquerytemp_0_Match_1] AS [matchtable] CROSS JOIN [SkyNode_Test].[dbo].[CatalogC] [c]", ReplaceXMatchTableSourcesTestHelper(sql));
        }
    }
}
