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
                    }
                };
            }
        }

        protected Jhu.Graywulf.SqlParser.TableReference HtmTable
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

        protected Jhu.Graywulf.SqlParser.TableReference HtmInner
        {
            get
            {
                return new Jhu.Graywulf.SqlParser.TableReference()
                {
                    DatabaseObjectName = "htminner",
                    Alias = "__htm_inner"
                };
            }
        }

        protected Jhu.Graywulf.SqlParser.TableReference HtmPartial
        {
            get
            {
                return new Jhu.Graywulf.SqlParser.TableReference()
                {
                    DatabaseObjectName = "htmpartial",
                    Alias = "__htm_partial"
                };
            }
        }

        protected SelectStatement Parse(string sql)
        {
            var p = new SkyQueryParser();
            return (SelectStatement)p.Execute(sql);
        }

        protected override SqlQuery CreateQuery(string query)
        {
            var q = base.CreateQuery(query);
            return q;
        }

        [TestMethod]
        public void GenerateHtmJoinConditionTest()
        {
            var sql = @"
SELECT *
FROM table WITH (HTMID(htmid))";

            var ss = Parse(sql);
            var ts = ss.EnumerateSourceTables(false).First();

            var sc = CallMethod(CodeGenerator, "GenerateHtmJoinCondition", ts, HtmTable);

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

            var gt = "@r.ContainsXyz(cx, cy, cz) = 1";

            Assert.AreEqual(gt, GenerateRegionContainsConditionTestHelper(sql));
        }

        [TestMethod]
        public void GenerateRegionContainsConditionEqTest()
        {
            var sql = @"
SELECT *
FROM table WITH (POINT(ra, dec))";

            var gt = "@r.ContainsEq(ra, dec) = 1";

            Assert.AreEqual(gt, GenerateRegionContainsConditionTestHelper(sql));
        }

        private string AppendRegionJoinsAndConditionsTestHelper(string sql, bool partial)
        {
            var ss = Parse(sql);
            var qs = ss.EnumerateQuerySpecifications().First();

            CallMethod(CodeGenerator, "AppendRegionJoinsAndConditions", qs, -1, HtmTable, partial);
            CallMethod(CodeGenerator, "RemoveNonStandardTokens", qs);

            return CodeGenerator.Execute(qs);
        }

        [TestMethod]
        public void AppendRegionJoinsAndConditionsQuerySpecificationTest()
        {
            var sql = @"
SELECT table.*
FROM table WITH (HTMID(htmid))
REGION 'CIRCLE J2000 10 10 10'";

            var gt = @"SELECT table.*
FROM table 
INNER JOIN htmtable AS [__htm]
ON htmid BETWEEN __htm.HtmIDStart AND __htm.HtmIDEnd
";

            Assert.AreEqual(gt, AppendRegionJoinsAndConditionsTestHelper(sql, false));
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
@"SELECT table.*
FROM htmtable AS [__htm]
INNER JOIN table 
ON htmid BETWEEN __htm.HtmIDStart AND __htm.HtmIDEnd
CROSS JOIN table2
CROSS JOIN table3
CROSS JOIN table4
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

            var gt = @"SELECT table.*
FROM table2
CROSS JOIN table 
INNER JOIN htmtable AS [__htm]
ON htmid BETWEEN __htm.HtmIDStart AND __htm.HtmIDEnd
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

            var gt = @"SELECT table.*
FROM table 
INNER JOIN htmtable AS [__htm]
ON htmid BETWEEN __htm.HtmIDStart AND __htm.HtmIDEnd
WHERE @r.ContainsEq(ra, dec) = 1
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

            var gt = @"SELECT table.*
FROM table 
WHERE @r.ContainsEq(ra, dec) = 1
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

            var gt = @"SELECT table.*
FROM table
";

            Assert.AreEqual(gt, AppendRegionJoinsAndConditionsTestHelper(sql, false));

        }

        private string AppendRegionJoinsAndConditionsTestHelper2(string sql)
        {
            var ss = Parse(sql);
            var htminner = new List<Graywulf.SqlParser.TableReference>() { HtmInner, HtmInner };
            var htmpartial = new List<Graywulf.SqlParser.TableReference>() { HtmPartial, HtmPartial };

            CallMethod(CodeGenerator, "AppendRegionJoinsAndConditions", ss, htminner, htmpartial);
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
((SELECT table.*
FROM table 
INNER JOIN htminner AS [__htm_inner]
ON htmid BETWEEN __htm_inner.HtmIDStart AND __htm_inner.HtmIDEnd
)
UNION ALL
(SELECT table.*
FROM table 
INNER JOIN htmpartial AS [__htm_partial]
ON htmid BETWEEN __htm_partial.HtmIDStart AND __htm_partial.HtmIDEnd
WHERE @r0.ContainsEq(ra, dec) = 1
))";

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
SELECT table.*
FROM table 
WHERE @r0.ContainsEq(ra, dec) = 1
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
((SELECT table.*
FROM table 
INNER JOIN htminner AS [__htm_inner]
ON htmid BETWEEN __htm_inner.HtmIDStart AND __htm_inner.HtmIDEnd
)
UNION ALL
(SELECT table.*
FROM table 
INNER JOIN htmpartial AS [__htm_partial]
ON htmid BETWEEN __htm_partial.HtmIDStart AND __htm_partial.HtmIDEnd
WHERE @r1.ContainsEq(ra, dec) = 1
))
UNION
((SELECT table2.*
FROM table2 
INNER JOIN htminner AS [__htm_inner]
ON htmid BETWEEN __htm_inner.HtmIDStart AND __htm_inner.HtmIDEnd
)
UNION ALL
(SELECT table2.*
FROM table2 
INNER JOIN htmpartial AS [__htm_partial]
ON htmid BETWEEN __htm_partial.HtmIDStart AND __htm_partial.HtmIDEnd
WHERE @r0.ContainsEq(ra, dec) = 1
))";

            Assert.AreEqual(gt, AppendRegionJoinsAndConditionsTestHelper2(sql));
        }

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

            var gt = @"WITH __t AS
(
		SELECT [objId], [ra], [dec]
	FROM [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll]
	INNER JOIN [$codedb].htm.Cover(@r) __htm
		ON [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[htmId] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd AND __htm.partial = 0
	WHERE [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[ra] > 2

	UNION ALL

	SELECT [objId], [ra], [dec]
	FROM [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll]
	INNER JOIN [$codedb].htm.Cover(@r) __htm
		ON [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[htmId] BETWEEN __htm.htmIDStart AND __htm.htmIDEnd AND __htm.partial = 1
	WHERE (@r.ContainsEq([SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[ra], [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[dec]) = 1) AND ([SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[ra] > 2)
)
INSERT [Graywulf_Temp].[dbo].[test__stat_TEST_dbo_SDSSDR7PhotoObjAll] WITH(TABLOCKX)
SELECT ROW_NUMBER() OVER (ORDER BY [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[dec]), [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[dec]
FROM __t;";

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

            var gt = @"WITH __t AS
(
		SELECT [objId], [ra], [dec]
	FROM [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll]
	WHERE (@r.ContainsEq([SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[ra], [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[dec]) = 1) AND ([SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[ra] > 2)
)
INSERT [Graywulf_Temp].[dbo].[test__stat_TEST_dbo_SDSSDR7PhotoObjAll] WITH(TABLOCKX)
SELECT ROW_NUMBER() OVER (ORDER BY [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[dec]), [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[dec]
FROM __t;";

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
