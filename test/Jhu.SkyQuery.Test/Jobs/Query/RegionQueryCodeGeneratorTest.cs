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
using Jhu.Graywulf.Jobs.Query;
using Jhu.Graywulf.Schema;
using Jhu.SkyQuery.Parser;
using Jhu.SkyQuery.Jobs.Query;

namespace Jhu.SkyQuery.Jobs.Query.Test
{
    [TestClass]
    public class RegionQueryCodeGeneratorTest : SkyQueryTestBase
    {
        private RegionQueryCodeGenerator CodeGenerator
        {
            get
            {
                return new RegionQueryCodeGenerator()
                {
                    CodeDataset = new Graywulf.Schema.SqlServer.SqlServerDataset()
                    {
                        Name = "CODE",
                        ConnectionString = "data source=localhost;initial catalog=SkyQuery_Code",
                    },
                    ResolveNames = true,
                };
            }
        }

        private Jhu.Graywulf.SqlParser.TableReference HtmTable
        {
            get
            {
                return new Jhu.Graywulf.SqlParser.TableReference()
                {
                    DatabaseObjectName = "htmtable",
                    Alias = "__htm"
                };
            }
        }

        #region Helper functions

        private string GenerateRegionContainsConditionTestHelper(string sql)
        {
            var ss = Parse(sql);
            var ts = ss.EnumerateSourceTables(false).First();

            var sc = (Jhu.Graywulf.SqlParser.SearchCondition)CallMethod(CodeGenerator, "GenerateRegionContainsCondition", ts, -1);

            return CodeGenerator.Execute(sc);
        }

        private string AppendRegionJoinsAndConditionsTestHelper(string sql, bool partial)
        {
            var ss = Parse(sql);
            var qs = ss.FindDescendantRecursive<Graywulf.SqlParser.QuerySpecification>();
            var regions = new List<Spherical.Region>();

            CallMethod(CodeGenerator, "AppendRegionJoinsAndConditions", ss, regions);
            CallMethod(CodeGenerator, "RemoveNonStandardTokens", qs);

            return CodeGenerator.Execute(ss);
        }

        private string AppendRegionJoinsAndConditionsTestHelper2(string sql)
        {
            var ss = Parse(sql);
            var regions = new List<Spherical.Region>();

            CallMethod(CodeGenerator, "AppendRegionJoinsAndConditions", ss, regions);
            CallMethod(CodeGenerator, "RemoveNonStandardTokens", ss);

            return CodeGenerator.Execute(ss);
        }

        private Graywulf.IO.Tasks.SourceTableQuery GetExecuteQueryTestHelper(string sql)
        {
            var q = CreateQuery(sql);
            var sq = CodeGenerator.GetExecuteQuery(q.SelectStatement);
            return sq;
        }

        // TODO: propagate up to test base class
        protected string GetStatisticsQuery(string sql)
        {
            var q = CreateQuery(sql);
            var cg = new RegionQueryCodeGenerator(q);
            var ts = q.SelectStatement.EnumerateQuerySpecifications().First().EnumerateSourceTables(false).First();

            // Fake that dec is used as a partitioning key
            var keycol = ts.TableReference.ColumnReferences.First(c => c.ColumnName == "dec");
            keycol.ColumnContext |= Graywulf.SqlParser.ColumnContext.SelectList;

            ts.TableReference.Statistics = new Graywulf.SqlParser.TableStatistics()
            {
                BinCount = 200,
                KeyColumn = Graywulf.SqlParser.Expression.Create(keycol),
                KeyColumnDataType = DataTypes.SqlFloat
            };

            var cmd = cg.GetTableStatisticsCommand(ts);

            return cmd.CommandText;
        }

        #endregion
        #region Detached tests

        [TestMethod]
        public void GenerateHtmJoinConditionTest()
        {
            var sql = @"
SELECT *
FROM table WITH (HTMID(htmid))";

            var ss = Parse(sql);
            var ts = ss.EnumerateSourceTables(false).First();

            var sc = CallMethod(CodeGenerator, "GenerateHtmJoinCondition", ts, HtmTable, true, 0);
            Assert.AreEqual("htmid BETWEEN __htm.HtmIDStart AND __htm.HtmIDEnd AND __htm.Partial = 1", sc.ToString());

            sc = CallMethod(CodeGenerator, "GenerateHtmJoinCondition", ts, HtmTable, false, 0);
            Assert.AreEqual("htmid BETWEEN __htm.HtmIDStart AND __htm.HtmIDEnd AND __htm.Partial = 0", sc.ToString());
        }
        
        [TestMethod]
        public void GenerateRegionContainsConditionXyzTest()
        {
            var sql = @"
SELECT *
FROM table WITH (POINT(cx, cy, cz))";

            var gt = "@r.ContainsXyz([cx], [cy], [cz]) = 1";

            Assert.AreEqual(gt, GenerateRegionContainsConditionTestHelper(sql));
        }

        [TestMethod]
        public void GenerateRegionContainsConditionEqTest()
        {
            var sql = @"
SELECT *
FROM table WITH (POINT(ra, dec))";

            var gt = "@r.ContainsEq([ra], [dec]) = 1";

            Assert.AreEqual(gt, GenerateRegionContainsConditionTestHelper(sql));
        }
        
        [TestMethod]
        public void AppendRegionJoinsAndConditionsQuerySpecificationTest()
        {
            var sql = @"
SELECT table.*
FROM table WITH (HTMID(htmid))
REGION 'CIRCLE J2000 10 10 10'";

            var gt = @"
SELECT [table].*
FROM ((SELECT [table].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [table] 
ON [table].[htmid] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 0)
UNION ALL
(SELECT [table].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [table] 
ON [table].[htmid] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 1)) AS [__htm_t0]
";

            var res = AppendRegionJoinsAndConditionsTestHelper(sql, false);

            Assert.AreEqual(gt, res);
        }

        [TestMethod]
        public void AppendRegionJoinsAndConditionsWithJoinedTableTest()
        {
            // Append HTM to the first table in FROM clause

            var sql = @"
SELECT table.*
FROM table WITH (HTMID(htmid))
CROSS JOIN table2
CROSS JOIN table3
CROSS JOIN table4
REGION 'CIRCLE J2000 10 10 10'";

            var gt =
@"
SELECT [table].*
FROM ((SELECT [table].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [table] 
ON [table].[htmid] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 0)
UNION ALL
(SELECT [table].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [table] 
ON [table].[htmid] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 1)) AS [__htm_t0]
CROSS JOIN [table2]
CROSS JOIN [table3]
CROSS JOIN [table4]
";

            Assert.AreEqual(gt, AppendRegionJoinsAndConditionsTestHelper(sql, false));
        }

        [TestMethod]
        public void AppendRegionJoinsAndConditionsWithJoinedTable2Test()
        {
            // Append HTM to the last table

            var sql = @"
SELECT table.*
FROM table2
CROSS JOIN table WITH (HTMID(htmid))
REGION 'CIRCLE J2000 10 10 10'";

            var gt = @"
SELECT [table].*
FROM [table2]
CROSS JOIN [table] 
";

            Assert.AreEqual(gt, AppendRegionJoinsAndConditionsTestHelper(sql, false));
        }

        [TestMethod]
        public void AppendRegionJoinsAndConditionsQuerySpecificationPartialTest()
        {
            var sql = @"
SELECT table.*
FROM table WITH (POINT(ra, dec), HTMID(htmid))
REGION 'CIRCLE J2000 10 10 10'";

            var gt = @"
SELECT [table].*
FROM ((SELECT [table].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [table] 
ON [table].[htmid] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 0)
UNION ALL
(SELECT [table].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [table] 
ON [table].[htmid] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 1 AND @r0.ContainsEq([table].[ra], [table].[dec]) = 1)) AS [__htm_t0]
";

#if false


SELECT [table].*
FROM ((SELECT [table].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [table] 
ON [table].[htmid] BETWEEN [table].[HtmIDStart] AND [table].[HtmIDEnd] AND [table].[Partial] = 0)
UNION ALL
(SELECT [table].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [table] 
ON [table].[htmid] BETWEEN [table].[HtmIDStart] AND [table].[HtmIDEnd] AND [table].[Partial] = 1 AND @r0.ContainsEq([table].[ra], [table].[dec]) = 1)) AS [__htm_t0]

#endif

            var res = AppendRegionJoinsAndConditionsTestHelper(sql, true);
            Assert.AreEqual(gt, res);
        }

        [TestMethod]
        public void AppendRegionJoinsAndConditionsQuerySpecificationNoHtmTest()
        {
            var sql = @"
SELECT table.*
FROM table WITH (POINT(ra, dec))
REGION 'CIRCLE J2000 10 10 10'";

            var gt = @"
SELECT [table].*
FROM [table] 
WHERE @r0.ContainsEq([ra], [dec]) = 1
";

            Assert.AreEqual(gt, AppendRegionJoinsAndConditionsTestHelper(sql, false));
        }

        [TestMethod]
        public void AppendRegionJoinsAndConditionsQuerySpecificationNoCoordinatesTest()
        {
            var sql = @"
SELECT table.*
FROM table
REGION 'CIRCLE J2000 10 10 10'";

            var gt = @"
SELECT [table].*
FROM [table]
";

            Assert.AreEqual(gt, AppendRegionJoinsAndConditionsTestHelper(sql, false));

        }

        [TestMethod]
        public void AppendRegionJoinsAndConditionsSelectStatementTest()
        {
            var sql = @"
SELECT table.*
FROM table WITH (POINT(ra, dec), HTMID(htmid))
REGION 'CIRCLE J2000 10 10 10'";

            var gt = @"
SELECT [table].*
FROM ((SELECT [table].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [table] 
ON [table].[htmid] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 0)
UNION ALL
(SELECT [table].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [table] 
ON [table].[htmid] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 1 AND @r0.ContainsEq([table].[ra], [table].[dec]) = 1)) AS [__htm_t0]
";

            Assert.AreEqual(gt, AppendRegionJoinsAndConditionsTestHelper2(sql));
        }

        [TestMethod]
        public void AppendRegionJoinsAndConditionsSelectStatementNoHtmTest()
        {
            var sql = @"
SELECT table.*
FROM table WITH (POINT(ra, dec))
REGION 'CIRCLE J2000 10 10 10'";

            var gt = @"
SELECT [table].*
FROM [table] 
WHERE @r0.ContainsEq([ra], [dec]) = 1
";

            Assert.AreEqual(gt, AppendRegionJoinsAndConditionsTestHelper2(sql));
        }

        [TestMethod]
        public void AppendRegionJoinsAndConditionsUnionQuery()
        {
            var sql = @"
SELECT table.*
FROM table WITH (POINT(ra, dec), HTMID(htmid))
REGION 'CIRCLE J2000 10 10 10'
UNION
SELECT table2.*
FROM table2 WITH (POINT(ra, dec), HTMID(htmid))
REGION 'CIRCLE J2000 10 10 10'";

            var gt = @"
SELECT [table].*
FROM ((SELECT [table].*
FROM [SkyQuery_Code].[htm].[Cover](@r1) AS [__htm1]
INNER LOOP JOIN [table] 
ON [table].[htmid] BETWEEN [__htm1].[HtmIDStart] AND [__htm1].[HtmIDEnd] AND [__htm1].[Partial] = 0)
UNION ALL
(SELECT [table].*
FROM [SkyQuery_Code].[htm].[Cover](@r1) AS [__htm1]
INNER LOOP JOIN [table] 
ON [table].[htmid] BETWEEN [__htm1].[HtmIDStart] AND [__htm1].[HtmIDEnd] AND [__htm1].[Partial] = 1 AND @r1.ContainsEq([table].[ra], [table].[dec]) = 1)) AS [__htm_t1]

UNION
SELECT [table2].*
FROM ((SELECT [table2].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [table2] 
ON [table2].[htmid] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 0)
UNION ALL
(SELECT [table2].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [table2] 
ON [table2].[htmid] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 1 AND @r0.ContainsEq([table2].[ra], [table2].[dec]) = 1)) AS [__htm_t0]
";

            Assert.AreEqual(gt, AppendRegionJoinsAndConditionsTestHelper2(sql));
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
FROM ((SELECT [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll]
ON [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[htmId] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 0)
UNION ALL
(SELECT [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll]
ON [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[htmId] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 1 AND @r0.ContainsXyz([SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[cx], [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[cy], [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[cz]) = 1)) AS [__htm_t0]
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
FROM ((SELECT [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll] 
ON [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[htmId] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 0)
UNION ALL
(SELECT [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll] 
ON [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[htmId] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 1 AND @r0.ContainsXyz([SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[cx], [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[cy], [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[cz]) = 1)) AS [__htm_t0]
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
SELECT [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll_NoHTM].[objId] AS [objId]
FROM [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll_NoHTM]
WHERE @r0.ContainsXyz([SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll_NoHTM].[cx], [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll_NoHTM].[cy], [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll_NoHTM].[cz]) = 1
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
SELECT [SkyNode_Test].[dbo].[SampleData_PrimaryKey].[ID] AS [ID]
FROM [SkyNode_Test].[dbo].[SampleData_PrimaryKey]
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
REGION 'CIRCLE J2000 0 0 10'
WHERE ra > 2";

            var gt = @"SELECT 
	FROM [$codedb].htm.Cover(@r) __htm
	INNER LOOP JOIN [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll]
		ON [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[htmId] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd AND __htm.partial = 0
	WHERE [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[ra] > 2

	UNION ALL

	SELECT 
	FROM [$codedb].htm.Cover(@r) __htm
	INNER LOOP JOIN [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll]
		ON [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[htmId] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd AND __htm.partial = 1
	WHERE (@r.ContainsXyz([SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[cx], [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[cy], [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[cz]) = 1) AND ([SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[ra] > 2)";

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
REGION 'CIRCLE J2000 0 0 10'
WHERE ra > 2";

            var gt = @"SELECT 
	FROM [$codedb].htm.Cover(@r) __htm
	INNER LOOP JOIN [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll]
		ON [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[htmId] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd AND __htm.partial = 0
	WHERE [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[ra] > 2

	UNION ALL

	SELECT 
	FROM [$codedb].htm.Cover(@r) __htm
	INNER LOOP JOIN [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll]
		ON [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[htmId] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd AND __htm.partial = 1
	WHERE (@r.ContainsXyz([SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[cx], [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[cy], [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[cz]) = 1) AND ([SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[ra] > 2)";

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
REGION 'CIRCLE J2000 0 0 10'
WHERE ra > 2";

            var gt = @"SELECT 
	FROM [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll_NoHTM]
	WHERE (@r.ContainsXyz([SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll_NoHTM].[cx], [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll_NoHTM].[cy], [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll_NoHTM].[cz]) = 1) AND ([SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll_NoHTM].[ra] > 2)";

            var res = GetStatisticsQuery(sql);
            Assert.IsTrue(res.Contains(gt));
        }

#endregion
    }
}
