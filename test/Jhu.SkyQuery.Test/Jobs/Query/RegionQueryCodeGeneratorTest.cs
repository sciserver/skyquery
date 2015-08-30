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


        [TestMethod]
        public void GenerateHtmJoinConditionTest(bool partial)
        {
            var sql = @"
SELECT *
FROM table WITH (HTMID(htmid))";

            var ss = Parse(sql);
            var ts = ss.EnumerateSourceTables(false).First();

            var sc = CallMethod(CodeGenerator, "GenerateHtmJoinCondition", ts, HtmTable, partial, 0);

            Assert.AreEqual("htmid BETWEEN __htm.HtmIDStart AND __htm.HtmIDEnd", sc.ToString());
        }

        private string GenerateRegionContainsConditionTestHelper(string sql)
        {
            var ss = Parse(sql);
            var ts = ss.EnumerateSourceTables(false).First();

            var sc = (Jhu.Graywulf.SqlParser.SearchCondition)CallMethod(CodeGenerator, "GenerateRegionContainsCondition", ts, -1);

            return CodeGenerator.Execute(sc);
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

        private string AppendRegionJoinsAndConditionsTestHelper(string sql, bool partial)
        {
            var ss = Parse(sql);
            var qs = ss.FindDescendantRecursive<Graywulf.SqlParser.QuerySpecification>();
            var regions = new List<Spherical.Region>();

            CallMethod(CodeGenerator, "AppendRegionJoinsAndConditions", ss, regions);
            CallMethod(CodeGenerator, "RemoveNonStandardTokens", qs);

            return CodeGenerator.Execute(ss);
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

            Assert.AreEqual(gt, AppendRegionJoinsAndConditionsTestHelper(sql, true));
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

        private string AppendRegionJoinsAndConditionsTestHelper2(string sql)
        {
            var ss = Parse(sql);
            var regions = new List<Spherical.Region>();

            CallMethod(CodeGenerator, "AppendRegionJoinsAndConditions", ss, regions);
            CallMethod(CodeGenerator, "RemoveNonStandardTokens", ss, CommandMethod.Select);

            return CodeGenerator.Execute(ss);
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

        #region

        private string GetExecuteQueryTestHelper(string sql)
        {
            var q = CreateQuery(sql);
            var sq = CodeGenerator.GetExecuteQuery(q.SelectStatement);
            return sq.Query;
        }

        [TestMethod]
        public void GetExecuteQueryWithHtmIDTest()
        {
            var sql = @"
SELECT *
FROM TEST:SDSSDR7PhotoObjAll WITH (POINT(ra, dec), HTMID(htmid))
REGION 'CIRCLE J2000 10 10 10'";

            var gt = @"DECLARE @r0 dbo.Region = @region0;

SELECT [__htm_t0].[objId] AS [objId], [__htm_t0].[skyVersion] AS [skyVersion], [__htm_t0].[run] AS [run], [__htm_t0].[rerun] AS [rerun], [__htm_t0].[camcol] AS [camcol], [__htm_t0].[field] AS [field], [__htm_t0].[obj] AS [obj], [__htm_t0].[mode] AS [mode], [__htm_t0].[type] AS [type], [__htm_t0].[ra] AS [ra], [__htm_t0].[dec] AS [dec], [__htm_t0].[raErr] AS [raErr], [__htm_t0].[decErr] AS [decErr], [__htm_t0].[cx] AS [cx], [__htm_t0].[cy] AS [cy], [__htm_t0].[cz] AS [cz], [__htm_t0].[htmId] AS [htmId], [__htm_t0].[zoneId] AS [zoneId], [__htm_t0].[u] AS [u], [__htm_t0].[g] AS [g], [__htm_t0].[r] AS [r], [__htm_t0].[i] AS [i], [__htm_t0].[z] AS [z]
FROM ((SELECT [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll] 
ON [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[htmId] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 0)
UNION ALL
(SELECT [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].*
FROM [SkyQuery_Code].[htm].[Cover](@r0) AS [__htm0]
INNER LOOP JOIN [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll] 
ON [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[htmId] BETWEEN [__htm0].[HtmIDStart] AND [__htm0].[HtmIDEnd] AND [__htm0].[Partial] = 1 AND @r0.ContainsEq([SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[ra], [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[dec]) = 1)) AS [__htm_t0]

";

            var res = GetExecuteQueryTestHelper(sql);
            Assert.AreEqual(gt, res);
        }

        #endregion
        #region Statistics query tests

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

        [TestMethod]
        public void GetTableStatisticsWithRegionCommandTest()
        {
            var sql = @"
SELECT objID
FROM TEST:SDSSDR7PhotoObjAll WITH (POINT(ra, dec), HTMID(htmid))
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
	WHERE (@r.ContainsEq([SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[ra], [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[dec]) = 1) AND ([SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[ra] > 2)";

            var res = GetStatisticsQuery(sql);
            Assert.IsTrue(res.Contains(gt));
        }

        [TestMethod]
        public void GetTableStatisticsWithRegionCommandNoHtmTest()
        {
            var sql = @"
SELECT objID
FROM TEST:SDSSDR7PhotoObjAll WITH (POINT(ra, dec))
REGION 'CIRCLE J2000 0 0 10'
WHERE ra > 2";

            var gt = @"SELECT 
	FROM [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll]
	WHERE (@r.ContainsEq([SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[ra], [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[dec]) = 1) AND ([SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[ra] > 2)";

            var res = GetStatisticsQuery(sql);
            Assert.IsTrue(res.Contains(gt));
        }

        [TestMethod]
        public void GetTableStatisticsWithRegionCommandNoCoordinatesTest()
        {
            var sql = @"
SELECT objID
FROM TEST:SDSSDR7PhotoObjAll
REGION 'CIRCLE J2000 0 0 10'
WHERE ra > 2";

            var gt = @"INSERT [Graywulf_Temp].[dbo].[test__stat_TEST_dbo_SDSSDR7PhotoObjAll] WITH(TABLOCKX)
SELECT ROW_NUMBER() OVER (ORDER BY [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[dec]), [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[dec]
FROM [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll]
WHERE [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[ra] > 2;";

            var res = GetStatisticsQuery(sql);
            Assert.IsTrue(res.Contains(gt));
        }

        #endregion
    }
}
