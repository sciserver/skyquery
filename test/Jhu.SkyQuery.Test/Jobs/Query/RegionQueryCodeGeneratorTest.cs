using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Test;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.Graywulf.Sql.Schema;
using Jhu.SkyQuery.Parser;
using Jhu.SkyQuery.Jobs.Query;

namespace Jhu.SkyQuery.Jobs.Query.Test
{
    [TestClass]
    public class RegionQueryCodeGeneratorTest : SkyQueryTestBase
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

        private Jhu.Graywulf.Sql.NameResolution.TableReference HtmTable
        {
            get
            {
                return new Jhu.Graywulf.Sql.NameResolution.TableReference()
                {
                    DatabaseObjectName = "htmtable",
                    Alias = "__htm"
                };
            }
        }

        #region Helper functions

        private Graywulf.Sql.Schema.SqlServer.SqlServerDataset GetCodeDataset()
        {
            return new Jhu.Graywulf.Sql.Schema.SqlServer.SqlServerDataset()
            {
                Name = "CODE",
                ConnectionString = "data source=localhost;initial catalog=SkyQuery_Code",
            };
        }

        private Graywulf.Sql.Schema.SqlServer.SqlServerDataset GetTestDataset()
        {
            return new Jhu.Graywulf.Sql.Schema.SqlServer.SqlServerDataset()
            {
                Name = "TEST",
                ConnectionString = "data source=localhost;initial catalog=SkyQuery_Schema_Test",
            };
        }

        protected override SchemaManager CreateSchemaManager()
        {
            var sm =base.CreateSchemaManager();
            sm.Datasets.TryAdd("TEST", GetTestDataset());
            return sm;
        }

        private RegionQueryCodeGenerator CreateCodeGenerator(QueryObject query)
        {
            return new RegionQueryCodeGenerator(query)
            {
                CodeDataset = GetCodeDataset(),
                ColumnNameRendering = Graywulf.Sql.CodeGeneration.NameRendering.FullyQualified,
                ColumnAliasRendering = Graywulf.Sql.CodeGeneration.AliasRendering.Always,
                TableNameRendering = Graywulf.Sql.CodeGeneration.NameRendering.FullyQualified,
                TableAliasRendering = Graywulf.Sql.CodeGeneration.AliasRendering.Always,
                FunctionNameRendering = Graywulf.Sql.CodeGeneration.NameRendering.FullyQualified,
            };
        }

        private string GenerateRegionContainsConditionTestHelper(string sql)
        {
            var q = CreateQuery(sql);
            var ts = q.QueryDetails.ParsingTree.FindDescendantRecursive<RegionSelectStatement>().QueryExpression.EnumerateSourceTables(false).First();
            var cg = CreateCodeGenerator(null);

            var sc = (Jhu.Graywulf.Sql.Parsing.BooleanExpression)CallMethod(cg, "GenerateRegionContainsCondition", ts, -1);
            

            return cg.Execute(sc);
        }

        private string AppendRegionJoinsAndConditionsTestHelper(string sql, bool partial)
        {
            var q = CreateQuery(sql);
            var ss = q.QueryDetails.ParsingTree.FindDescendantRecursive<RegionSelectStatement>();
            var qs = ss.FindDescendantRecursive<Graywulf.Sql.Parsing.QuerySpecification>();
            var regions = new List<Spherical.Region>();
            var cg = CreateCodeGenerator(null);

            CallMethod(cg, "AppendRegionJoinsAndConditions", ss, regions);
            CallMethod(cg, "RemoveNonStandardTokens", qs);

            return cg.Execute(q.QueryDetails.ParsingTree);
        }

        private string AppendRegionJoinsAndConditionsTestHelper2(string sql)
        {
            var q = CreateQuery(sql);
            var ss = q.QueryDetails.ParsingTree.FindDescendantRecursive<RegionSelectStatement>();
            var regions = new List<Spherical.Region>();
            var cg = CreateCodeGenerator(null);

            CallMethod(cg, "AppendRegionJoinsAndConditions", ss, regions);
            CallMethod(cg, "RemoveNonStandardTokens", ss);

            return cg.Execute(q.QueryDetails.ParsingTree);
        }

        private Graywulf.IO.Tasks.SourceQuery GetExecuteQueryTestHelper(string sql)
        {
            // Work in single server mode to avoid spinning up the entire
            // scheduler infrastructure
            var q = CreateQuery(sql);
            q.Parameters.ExecutionMode = ExecutionMode.SingleServer;
            q.GeneratePartitions();
            var qp = q.Partitions[0];
            var cg = CreateCodeGenerator(qp);
            var sq = cg.GetExecuteQuery();
            return sq;
        }

        // TODO: propagate up to test base class
        protected string GetStatisticsQuery(string sql)
        {
            var q = CreateQuery(sql);
            q.Parameters.ExecutionMode = ExecutionMode.SingleServer;

            var cg = CreateCodeGenerator(q);
            var ss = q.QueryDetails.ParsingTree.FindDescendantRecursive<RegionSelectStatement>();
            var ts = ss.QueryExpression.EnumerateQuerySpecifications().First().EnumerateSourceTables(false).First();

            // Fake that dec is used as a partitioning key
            var keycol = ts.TableReference.ColumnReferences.First(c => c.ColumnName == "dec");
            keycol.ColumnContext |= Graywulf.Sql.NameResolution.ColumnContext.SelectList;

            q.TableStatistics[ts] = new Graywulf.Sql.Jobs.Query.TableStatistics()
            {
                BinCount = 200,
                KeyColumn = Graywulf.Sql.Parsing.Expression.Create(keycol),
                KeyColumnDataType = Jhu.Graywulf.Sql.Schema.DataTypes.SqlFloat
            };

            var cmd = cg.GetTableStatisticsCommand(ts, null);

            return cmd.CommandText;
        }

        #endregion
        #region Detached tests

        [TestMethod]
        public void GenerateHtmJoinConditionTest()
        {
            var sql = @"
SELECT *
FROM table1 WITH (HTMID(htmid))
REGION 'CIRCLE J2000 10 10 10'";

            var script = Parse(sql);
            var ss = script.FindDescendantRecursive<RegionSelectStatement>();
            var ts = ss.QueryExpression.EnumerateSourceTables(false).First();
            var cg = CreateCodeGenerator(null);

            var sc = (Jhu.Graywulf.Parsing.Node)CallMethod(cg, "GenerateHtmJoinCondition", ts, HtmTable, true, 0);
            Assert.AreEqual("[htmid] BETWEEN [__htm].[HtmIDStart] AND [__htm].[HtmIDEnd] AND [__htm].[Partial] = 1", cg.Execute(sc));

            sc = (Jhu.Graywulf.Parsing.Node)CallMethod(cg, "GenerateHtmJoinCondition", ts, HtmTable, false, 0);
            Assert.AreEqual("[htmid] BETWEEN [__htm].[HtmIDStart] AND [__htm].[HtmIDEnd] AND [__htm].[Partial] = 0", cg.Execute(sc));
        }

        [TestMethod]
        public void GenerateRegionContainsConditionXyzTest()
        {
            var sql = @"
SELECT *
FROM TEST:CatalogA WITH (POINT(cx, cy, cz))
REGION 'CIRCLE J2000 10 10 10'";

            var gt = "@r.ContainsXyz([SkyNode_TEST].[dbo].[CatalogA].[cx], [SkyNode_TEST].[dbo].[CatalogA].[cy], [SkyNode_TEST].[dbo].[CatalogA].[cz]) = 1";

            var res = GenerateRegionContainsConditionTestHelper(sql);
            Assert.AreEqual(gt, res);
        }

        [TestMethod]
        public void GenerateRegionContainsConditionEqTest()
        {
            var sql = @"
SELECT *
FROM TEST:SampleData WITH (POINT([double], [double]))
REGION 'CIRCLE J2000 10 10 10'";

            var gt = "@r.ContainsEq([SkyNode_TEST].[dbo].[SampleData].[double], [SkyNode_TEST].[dbo].[SampleData].[double]) = 1";

            var res = GenerateRegionContainsConditionTestHelper(sql);
            Assert.AreEqual(gt, GenerateRegionContainsConditionTestHelper(sql));
        }

        [TestMethod]
        public void AppendRegionJoinsAndConditionsQuerySpecificationTest()
        {
            // TODO: this might be an invalid query because there's no point in
            // running a region query where only coarse filtering is possible

            var sql = @"
SELECT SampleData.*
FROM TEST:SampleData WITH (HTMID([bigint]))
REGION 'CIRCLE J2000 10 10 10'";

            var gt = @"
SELECT [__htm_t0].[float] AS [float], [__htm_t0].[double] AS [double], [__htm_t0].[decimal] AS [decimal], [__htm_t0].[nvarchar(50)] AS [nvarchar(50)], [__htm_t0].[bigint] AS [bigint], [__htm_t0].[int] AS [int], [__htm_t0].[tinyint] AS [tinyint], [__htm_t0].[smallint] AS [smallint], [__htm_t0].[bit] AS [bit], [__htm_t0].[ntext] AS [ntext], [__htm_t0].[char] AS [char], [__htm_t0].[datetime] AS [datetime], [__htm_t0].[guid] AS [guid]
FROM ((SELECT [SkyNode_TEST].[dbo].[SampleData].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [SkyNode_TEST].[dbo].[SampleData] 
ON [SkyNode_TEST].[dbo].[SampleData].[bigint] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 0)
UNION ALL
(SELECT [SkyNode_TEST].[dbo].[SampleData].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [SkyNode_TEST].[dbo].[SampleData] 
ON [SkyNode_TEST].[dbo].[SampleData].[bigint] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 1)) AS [__htm_t0]
";

            var res = AppendRegionJoinsAndConditionsTestHelper(sql, false);
            Assert.AreEqual(gt, res);
        }

        [TestMethod]
        public void AppendRegionJoinsAndConditionsWithJoinedTableTest()
        {
            // Append HTM to the first table in FROM clause

            var sql = @"
SELECT SampleData.*
FROM TEST:SampleData WITH (HTMID([bigint]))
CROSS JOIN TEST:SampleData_AllPrecision
CROSS JOIN TEST:SampleData_AllTypes
CROSS JOIN TEST:SampleData_NumericTypes
REGION 'CIRCLE J2000 10 10 10'";

            var gt =
@"
SELECT [__htm_t0].[float] AS [float], [__htm_t0].[double] AS [double], [__htm_t0].[decimal] AS [decimal], [__htm_t0].[nvarchar(50)] AS [nvarchar(50)], [__htm_t0].[bigint] AS [bigint], [__htm_t0].[int] AS [int], [__htm_t0].[tinyint] AS [tinyint], [__htm_t0].[smallint] AS [smallint], [__htm_t0].[bit] AS [bit], [__htm_t0].[ntext] AS [ntext], [__htm_t0].[char] AS [char], [__htm_t0].[datetime] AS [datetime], [__htm_t0].[guid] AS [guid]
FROM ((SELECT [SkyNode_TEST].[dbo].[SampleData].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [SkyNode_TEST].[dbo].[SampleData] 
ON [SkyNode_TEST].[dbo].[SampleData].[bigint] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 0)
UNION ALL
(SELECT [SkyNode_TEST].[dbo].[SampleData].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [SkyNode_TEST].[dbo].[SampleData] 
ON [SkyNode_TEST].[dbo].[SampleData].[bigint] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 1)) AS [__htm_t0]
CROSS JOIN [SkyNode_TEST].[dbo].[SampleData_AllPrecision]
CROSS JOIN [SkyNode_TEST].[dbo].[SampleData_AllTypes]
CROSS JOIN [SkyNode_TEST].[dbo].[SampleData_NumericTypes]
";

            var res = AppendRegionJoinsAndConditionsTestHelper(sql, false);
            Assert.AreEqual(gt, res);
        }

        [TestMethod]
        public void AppendRegionJoinsAndConditionsWithJoinedTable2Test()
        {
            // Append HTM to the last table

            var sql = @"
SELECT TEST:SampleData.*
FROM TEST:SampleData_NumericTypes
CROSS JOIN TEST:SampleData WITH (HTMID([bigint]))
REGION 'CIRCLE J2000 10 10 10'";

            var gt = @"
SELECT [SkyNode_TEST].[dbo].[SampleData].[float] AS [float], [SkyNode_TEST].[dbo].[SampleData].[double] AS [double], [SkyNode_TEST].[dbo].[SampleData].[decimal] AS [decimal], [SkyNode_TEST].[dbo].[SampleData].[nvarchar(50)] AS [nvarchar(50)], [SkyNode_TEST].[dbo].[SampleData].[bigint] AS [bigint], [SkyNode_TEST].[dbo].[SampleData].[int] AS [int], [SkyNode_TEST].[dbo].[SampleData].[tinyint] AS [tinyint], [SkyNode_TEST].[dbo].[SampleData].[smallint] AS [smallint], [SkyNode_TEST].[dbo].[SampleData].[bit] AS [bit], [SkyNode_TEST].[dbo].[SampleData].[ntext] AS [ntext], [SkyNode_TEST].[dbo].[SampleData].[char] AS [char], [SkyNode_TEST].[dbo].[SampleData].[datetime] AS [datetime], [SkyNode_TEST].[dbo].[SampleData].[guid] AS [guid]
FROM [SkyNode_TEST].[dbo].[SampleData_NumericTypes]
CROSS JOIN [SkyNode_TEST].[dbo].[SampleData] 
";

            var res = AppendRegionJoinsAndConditionsTestHelper(sql, false);
            Assert.AreEqual(gt, res);
            Assert.Fail();
        }

        [TestMethod]
        public void AppendRegionJoinsAndConditionsQuerySpecificationPartialTest()
        {
            var sql = @"
SELECT *
FROM TEST:CatalogA a WITH (POINT(ra, dec), HTMID(htmid))
REGION 'CIRCLE J2000 10 10 10'";

            var gt = @"
SELECT [__htm_t0].[objId] AS [a_objId], [__htm_t0].[ra] AS [a_ra], [__htm_t0].[dec] AS [a_dec], [__htm_t0].[astroErr] AS [a_astroErr], [__htm_t0].[cx] AS [a_cx], [__htm_t0].[cy] AS [a_cy], [__htm_t0].[cz] AS [a_cz], [__htm_t0].[htmId] AS [a_htmId], [__htm_t0].[zoneId] AS [a_zoneId], [__htm_t0].[mag_1] AS [a_mag_1], [__htm_t0].[mag_2] AS [a_mag_2], [__htm_t0].[mag_3] AS [a_mag_3]
FROM ((SELECT [a].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [SkyNode_TEST].[dbo].[CatalogA] [a] 
ON [a].[htmId] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 0)
UNION ALL
(SELECT [a].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [SkyNode_TEST].[dbo].[CatalogA] [a] 
ON [a].[htmId] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 1 AND @r0.ContainsEq([a].[ra], [a].[dec]) = 1)) AS [__htm_t0]
";

            var res = AppendRegionJoinsAndConditionsTestHelper(sql, true);
            Assert.AreEqual(gt, res);
        }

        [TestMethod]
        public void AppendRegionJoinsAndConditionsQuerySpecificationNoHtmTest()
        {
            var sql = @"
SELECT SampleData.*
FROM TEST:SampleData WITH (POINT([double], [double]))
REGION 'CIRCLE J2000 10 10 10'";

            var gt = @"
SELECT [SkyNode_TEST].[dbo].[SampleData].[float] AS [float], [SkyNode_TEST].[dbo].[SampleData].[double] AS [double], [SkyNode_TEST].[dbo].[SampleData].[decimal] AS [decimal], [SkyNode_TEST].[dbo].[SampleData].[nvarchar(50)] AS [nvarchar(50)], [SkyNode_TEST].[dbo].[SampleData].[bigint] AS [bigint], [SkyNode_TEST].[dbo].[SampleData].[int] AS [int], [SkyNode_TEST].[dbo].[SampleData].[tinyint] AS [tinyint], [SkyNode_TEST].[dbo].[SampleData].[smallint] AS [smallint], [SkyNode_TEST].[dbo].[SampleData].[bit] AS [bit], [SkyNode_TEST].[dbo].[SampleData].[ntext] AS [ntext], [SkyNode_TEST].[dbo].[SampleData].[char] AS [char], [SkyNode_TEST].[dbo].[SampleData].[datetime] AS [datetime], [SkyNode_TEST].[dbo].[SampleData].[guid] AS [guid]
FROM [SkyNode_TEST].[dbo].[SampleData] 
WHERE @r0.ContainsEq([SkyNode_TEST].[dbo].[SampleData].[double], [SkyNode_TEST].[dbo].[SampleData].[double]) = 1
";

            var res = AppendRegionJoinsAndConditionsTestHelper(sql, false);
            Assert.AreEqual(gt, res);
        }

        [TestMethod]
        public void AppendRegionJoinsAndConditionsQuerySpecificationNoCoordinatesTest()
        {
            // Region constraint is opt-in

            var sql = @"
SELECT SampleData.*
FROM TEST:SampleData
REGION 'CIRCLE J2000 10 10 10'";

            var gt = @"
SELECT [SkyNode_TEST].[dbo].[SampleData].[float] AS [float], [SkyNode_TEST].[dbo].[SampleData].[double] AS [double], [SkyNode_TEST].[dbo].[SampleData].[decimal] AS [decimal], [SkyNode_TEST].[dbo].[SampleData].[nvarchar(50)] AS [nvarchar(50)], [SkyNode_TEST].[dbo].[SampleData].[bigint] AS [bigint], [SkyNode_TEST].[dbo].[SampleData].[int] AS [int], [SkyNode_TEST].[dbo].[SampleData].[tinyint] AS [tinyint], [SkyNode_TEST].[dbo].[SampleData].[smallint] AS [smallint], [SkyNode_TEST].[dbo].[SampleData].[bit] AS [bit], [SkyNode_TEST].[dbo].[SampleData].[ntext] AS [ntext], [SkyNode_TEST].[dbo].[SampleData].[char] AS [char], [SkyNode_TEST].[dbo].[SampleData].[datetime] AS [datetime], [SkyNode_TEST].[dbo].[SampleData].[guid] AS [guid]
FROM [SkyNode_TEST].[dbo].[SampleData]
";
            var res = AppendRegionJoinsAndConditionsTestHelper(sql, false);
            Assert.AreEqual(gt, res);

        }

        [TestMethod]
        public void AppendRegionJoinsAndConditionsSelectStatementTest()
        {
            var sql = @"
SELECT SampleData.*
FROM TEST:SampleData WITH (POINT([double], [double]), HTMID([bigint]))
REGION 'CIRCLE J2000 10 10 10'";

            var gt = @"
SELECT [__htm_t0].[float] AS [float], [__htm_t0].[double] AS [double], [__htm_t0].[decimal] AS [decimal], [__htm_t0].[nvarchar(50)] AS [nvarchar(50)], [__htm_t0].[bigint] AS [bigint], [__htm_t0].[int] AS [int], [__htm_t0].[tinyint] AS [tinyint], [__htm_t0].[smallint] AS [smallint], [__htm_t0].[bit] AS [bit], [__htm_t0].[ntext] AS [ntext], [__htm_t0].[char] AS [char], [__htm_t0].[datetime] AS [datetime], [__htm_t0].[guid] AS [guid]
FROM ((SELECT [SkyNode_TEST].[dbo].[SampleData].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [SkyNode_TEST].[dbo].[SampleData] 
ON [SkyNode_TEST].[dbo].[SampleData].[bigint] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 0)
UNION ALL
(SELECT [SkyNode_TEST].[dbo].[SampleData].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [SkyNode_TEST].[dbo].[SampleData] 
ON [SkyNode_TEST].[dbo].[SampleData].[bigint] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 1 AND @r0.ContainsEq([SkyNode_TEST].[dbo].[SampleData].[double], [SkyNode_TEST].[dbo].[SampleData].[double]) = 1)) AS [__htm_t0]
";

            var res = AppendRegionJoinsAndConditionsTestHelper2(sql);
            Assert.AreEqual(gt, res);
        }

        [TestMethod]
        public void AppendRegionJoinsAndConditionsSelectStatementNoHtmTest()
        {
            var sql = @"
SELECT SampleData.*
FROM TEST:SampleData WITH (POINT([double], [double]))
REGION 'CIRCLE J2000 10 10 10'";

            var gt = @"
SELECT [SkyNode_TEST].[dbo].[SampleData].[float] AS [float], [SkyNode_TEST].[dbo].[SampleData].[double] AS [double], [SkyNode_TEST].[dbo].[SampleData].[decimal] AS [decimal], [SkyNode_TEST].[dbo].[SampleData].[nvarchar(50)] AS [nvarchar(50)], [SkyNode_TEST].[dbo].[SampleData].[bigint] AS [bigint], [SkyNode_TEST].[dbo].[SampleData].[int] AS [int], [SkyNode_TEST].[dbo].[SampleData].[tinyint] AS [tinyint], [SkyNode_TEST].[dbo].[SampleData].[smallint] AS [smallint], [SkyNode_TEST].[dbo].[SampleData].[bit] AS [bit], [SkyNode_TEST].[dbo].[SampleData].[ntext] AS [ntext], [SkyNode_TEST].[dbo].[SampleData].[char] AS [char], [SkyNode_TEST].[dbo].[SampleData].[datetime] AS [datetime], [SkyNode_TEST].[dbo].[SampleData].[guid] AS [guid]
FROM [SkyNode_TEST].[dbo].[SampleData] 
WHERE @r0.ContainsEq([SkyNode_TEST].[dbo].[SampleData].[double], [SkyNode_TEST].[dbo].[SampleData].[double]) = 1
";

            var res = AppendRegionJoinsAndConditionsTestHelper2(sql);
            Assert.AreEqual(gt, res);
        }

        [TestMethod]
        public void AppendRegionJoinsAndConditionsUnionQuery()
        {
            var sql = @"
SELECT CatalogA.*
FROM TEST:CatalogA WITH (POINT(ra, dec), HTMID(htmid))
REGION 'CIRCLE J2000 10 10 10'
UNION
SELECT CatalogB.*
FROM TEST:CatalogB WITH (POINT(ra, dec), HTMID(htmid))
REGION 'CIRCLE J2000 10 10 10'";

            var gt = @"
SELECT [__htm_t1].[objId] AS [objId], [__htm_t1].[ra] AS [ra], [__htm_t1].[dec] AS [dec], [__htm_t1].[astroErr] AS [astroErr], [__htm_t1].[cx] AS [cx], [__htm_t1].[cy] AS [cy], [__htm_t1].[cz] AS [cz], [__htm_t1].[htmId] AS [htmId], [__htm_t1].[zoneId] AS [zoneId], [__htm_t1].[mag_1] AS [mag_1], [__htm_t1].[mag_2] AS [mag_2], [__htm_t1].[mag_3] AS [mag_3]
FROM ((SELECT [SkyNode_TEST].[dbo].[CatalogA].*
FROM [SkyQuery_Code].[htm].[Cover](@r1) AS [__htm1]
INNER LOOP JOIN [SkyNode_TEST].[dbo].[CatalogA] 
ON [SkyNode_TEST].[dbo].[CatalogA].[htmId] BETWEEN [__htm1].[HtmIDStart] AND [__htm1].[HtmIDEnd] AND [__htm1].[Partial] = 0)
UNION ALL
(SELECT [SkyNode_TEST].[dbo].[CatalogA].*
FROM [SkyQuery_Code].[htm].[Cover](@r1) AS [__htm1]
INNER LOOP JOIN [SkyNode_TEST].[dbo].[CatalogA] 
ON [SkyNode_TEST].[dbo].[CatalogA].[htmId] BETWEEN [__htm1].[HtmIDStart] AND [__htm1].[HtmIDEnd] AND [__htm1].[Partial] = 1 AND @r1.ContainsEq([SkyNode_TEST].[dbo].[CatalogA].[ra], [SkyNode_TEST].[dbo].[CatalogA].[dec]) = 1)) AS [__htm_t1]

UNION
SELECT [__htm_t0].[objId] AS [objId], [__htm_t0].[ra] AS [ra], [__htm_t0].[dec] AS [dec], [__htm_t0].[astroErr] AS [astroErr], [__htm_t0].[cx] AS [cx], [__htm_t0].[cy] AS [cy], [__htm_t0].[cz] AS [cz], [__htm_t0].[htmId] AS [htmId], [__htm_t0].[zoneId] AS [zoneId], [__htm_t0].[mag_1] AS [mag_1], [__htm_t0].[mag_2] AS [mag_2], [__htm_t0].[mag_3] AS [mag_3]
FROM ((SELECT [SkyNode_TEST].[dbo].[CatalogB].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [SkyNode_TEST].[dbo].[CatalogB] 
ON [SkyNode_TEST].[dbo].[CatalogB].[htmId] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 0)
UNION ALL
(SELECT [SkyNode_TEST].[dbo].[CatalogB].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [SkyNode_TEST].[dbo].[CatalogB] 
ON [SkyNode_TEST].[dbo].[CatalogB].[htmId] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 1 AND @r0.ContainsEq([SkyNode_TEST].[dbo].[CatalogB].[ra], [SkyNode_TEST].[dbo].[CatalogB].[dec]) = 1)) AS [__htm_t0]
";

            var res = AppendRegionJoinsAndConditionsTestHelper2(sql);
            Assert.AreEqual(gt, res);
        }

        #endregion
        #region Database-bound HTMID tests

        [TestMethod]
        public void GetExecuteQuery_NoHints_Test()
        {
            var sql = @"
SELECT objID
FROM TEST:SDSSDR7PhotoObjAll
REGION 'CIRCLE J2000 10 10 10'";

            var gt1 = @"DECLARE @r0 dbo.Region = @region0;
";

            var gt2 = @"
SELECT [__htm_t0].[objId] AS [objId]
FROM ((SELECT [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll]
ON [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[htmId] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 0)
UNION ALL
(SELECT [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll]
ON [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[htmId] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 1 AND @r0.ContainsXyz([SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[cx], [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[cy], [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[cz]) = 1)) AS [__htm_t0]
";

            var res = GetExecuteQueryTestHelper(sql);
            Assert.AreEqual(gt1, res.Header);
            Assert.AreEqual(gt2, res.Query);
        }

        [TestMethod]
        public void GetExecuteQuery_AllHints_Test()
        {
            var sql = @"
SELECT objID
FROM TEST:SDSSDR7PhotoObjAll WITH (POINT(ra, dec, cx, cy, cz), HTMID(htmid))
REGION 'CIRCLE J2000 10 10 10'";

            var gt1 = @"DECLARE @r0 dbo.Region = @region0;
";

            var gt2 = @"
SELECT [__htm_t0].[objId] AS [objId]
FROM ((SELECT [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll] 
ON [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[htmId] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 0)
UNION ALL
(SELECT [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll] 
ON [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[htmId] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 1 AND @r0.ContainsXyz([SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[cx], [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[cy], [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[cz]) = 1)) AS [__htm_t0]
";

            var res = GetExecuteQueryTestHelper(sql);
            Assert.AreEqual(gt1, res.Header);
            Assert.AreEqual(gt2, res.Query);
        }

        [TestMethod]
        public void GetExecuteQuery_NoHtm_Test()
        {
            var sql = @"
SELECT objID
FROM TEST:SDSSDR7PhotoObjAll_NoHtm
REGION 'CIRCLE J2000 10 10 10'";

            var gt1 = @"DECLARE @r0 dbo.Region = @region0;
";

            var gt2 = @"
SELECT [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll_NoHTM].[objId] AS [objId]
FROM [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll_NoHTM]
WHERE @r0.ContainsXyz([SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll_NoHTM].[cx], [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll_NoHTM].[cy], [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll_NoHTM].[cz]) = 1
";

            var res = GetExecuteQueryTestHelper(sql);
            Assert.AreEqual(gt1, res.Header);
            Assert.AreEqual(gt2, res.Query);
        }

        [TestMethod]
        public void GetExecuteQuery_NoCoordinates_Test()
        {
            var sql = @"
SELECT ID
FROM TEST:SampleData_PrimaryKey
REGION 'CIRCLE J2000 10 10 10'";

            var gt1 = @"DECLARE @r0 dbo.Region = @region0;
";

            var gt2 = @"
SELECT [SkyNode_TEST].[dbo].[SampleData_PrimaryKey].[ID] AS [ID]
FROM [SkyNode_TEST].[dbo].[SampleData_PrimaryKey]
";

            var res = GetExecuteQueryTestHelper(sql);
            Assert.AreEqual(gt1, res.Header);
            Assert.AreEqual(gt2, res.Query);
        }

        #endregion
        #region Database-bound statistics query tests

        [TestMethod]
        public void GetTableStatisticsWithRegion_NoHints_Test()
        {
            // NOTE: should this generate a fake query with no valid select list

            var sql = @"
SELECT objID
FROM TEST:SDSSDR7PhotoObjAll
WHERE ra > 2
REGION 'CIRCLE J2000 0 0 10'";

            var gt = @"		SELECT 
	FROM [$codedb].htm.Cover(@r) __htm
	INNER LOOP JOIN [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll]
		ON [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[htmId] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd AND __htm.partial = 0
	WHERE ([SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[ra] > 2)

	UNION ALL

	SELECT 
	FROM [$codedb].htm.Cover(@r) __htm
	INNER LOOP JOIN [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll]
		ON [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[htmId] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd AND __htm.partial = 1
	WHERE (@r.ContainsXyz([SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[cx], [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[cy], [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[cz]) = 1) AND (([SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[ra] > 2))";

            var res = GetStatisticsQuery(sql);
            Assert.IsTrue(res.Contains(gt));
        }

        [TestMethod]
        public void GetTableStatisticsWithRegion_AllHints_Test()
        {
            // NOTE: should this generate a fake query with no valid select list

            var sql = @"
SELECT objID
FROM TEST:SDSSDR7PhotoObjAll WITH (POINT(ra, dec, cx, cy, cz), HTMID(htmId))
WHERE ra > 2
REGION 'CIRCLE J2000 0 0 10'";

            var gt = @"SELECT 
	FROM [$codedb].htm.Cover(@r) __htm
	INNER LOOP JOIN [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll]
		ON [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[htmId] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd AND __htm.partial = 0
	WHERE ([SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[ra] > 2)

	UNION ALL

	SELECT 
	FROM [$codedb].htm.Cover(@r) __htm
	INNER LOOP JOIN [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll]
		ON [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[htmId] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd AND __htm.partial = 1
	WHERE (@r.ContainsXyz([SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[cx], [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[cy], [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[cz]) = 1) AND (([SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll].[ra] > 2))";

            var res = GetStatisticsQuery(sql);
            Assert.IsTrue(res.Contains(gt));
        }

        [TestMethod]
        public void GetTableStatisticsWithRegion_NoHtm_Test()
        {
            // NOTE: should this generate a fake query with no valid select list

            var sql = @"
SELECT objID
FROM TEST:SDSSDR7PhotoObjAll_NoHtm
WHERE ra > 2
REGION 'CIRCLE J2000 0 0 10'";

            var gt = @"SELECT 
	FROM [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll_NoHTM]
	WHERE (@r.ContainsXyz([SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll_NoHTM].[cx], [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll_NoHTM].[cy], [SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll_NoHTM].[cz]) = 1) AND (([SkyNode_TEST].[dbo].[SDSSDR7PhotoObjAll_NoHTM].[ra] > 2))";

            var res = GetStatisticsQuery(sql);
            Assert.IsTrue(res.Contains(gt));
        }

        #endregion
    }
}
