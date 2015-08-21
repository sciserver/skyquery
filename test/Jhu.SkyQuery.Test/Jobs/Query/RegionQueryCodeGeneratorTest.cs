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
        public void CreateHtmJoinConditionTest()
        {
            var sql = @"
SELECT *
FROM table WITH (HTMID(htmid))";

            var ss = Parse(sql);
            var ts = ss.EnumerateSourceTables(false).First();

            var sc = CallMethod(CodeGenerator, "CreateHtmJoinCondition", ts, HtmTable);

            Assert.AreEqual("htmid BETWEEN __htm.HtmIDStart AND __htm.HtmIDEnd", sc.ToString());
        }

        private string CreateRegionContainsConditionTestHelper(string sql)
        {
            var ss = Parse(sql);
            var ts = ss.EnumerateSourceTables(false).First();

            var sc = (Jhu.Graywulf.SqlParser.SearchCondition)CallMethod(CodeGenerator, "CreateRegionContainsCondition", ts, -1);

            return CodeGenerator.Execute(sc);
        }


        [TestMethod]
        public void CreateRegionContainsConditionXyzTest()
        {
            var sql = @"
SELECT *
FROM table WITH (POINT(cx, cy, cz))";

            var gt = "@r.ContainsXyz(cx, cy, cz) = 1";

            Assert.AreEqual(gt, CreateRegionContainsConditionTestHelper(sql));
        }

        [TestMethod]
        public void CreateRegionContainsConditionEqTest()
        {
            var sql = @"
SELECT *
FROM table WITH (POINT(ra, dec))";

            var gt = "@r.ContainsEq(ra, dec) = 1";

            Assert.AreEqual(gt, CreateRegionContainsConditionTestHelper(sql));
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

            ts.TableReference.Statistics = new Graywulf.SqlParser.TableStatistics()
            {
                BinCount = 200,
                KeyColumn = Graywulf.SqlParser.Expression.Create(new Graywulf.SqlParser.ColumnReference("dec", DataTypes.SqlFloat))
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

            var gt = @"INSERT [Graywulf_Temp].[dbo].[test__stat_TEST_dbo_SDSSDR7PhotoObjAll] WITH(TABLOCKX)
SELECT ROW_NUMBER() OVER (ORDER BY [dec]), [dec]
FROM [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll]
INNER JOIN [Graywulf_Temp].[dbo].[test__htm_TEST_dbo_SDSSDR7PhotoObjAll] __htm
	ON htmid BETWEEN __htm.htmIDStart AND __htm.htmIDEnd
WHERE [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[ra] > 2;";

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

            var gt = @"INSERT [Graywulf_Temp].[dbo].[test__stat_TEST_dbo_SDSSDR7PhotoObjAll] WITH(TABLOCKX)
SELECT ROW_NUMBER() OVER (ORDER BY [dec]), [dec]
FROM [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll]
WHERE (@r.ContainsEq([SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[ra], [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[dec]) = 1) AND ([SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[ra] > 2);";

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
SELECT ROW_NUMBER() OVER (ORDER BY [dec]), [dec]
FROM [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll]
WHERE [SkyNode_Test].[dbo].[SDSSDR7PhotoObjAll].[ra] > 2;";

            var res = GetStatisticsQuery(sql);
            Assert.IsTrue(res.Contains(gt));
        }

        #endregion
    }
}
