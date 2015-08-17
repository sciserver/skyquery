using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Test;
using Jhu.Graywulf.Jobs.Query;
using Jhu.Graywulf.Schema;
using Jhu.SkyQuery.Parser;
using Jhu.SkyQuery.Jobs.Query;

namespace Jhu.SkyQuery.Jobs.Query
{
    [TestClass]
    public class RegionQueryCodeGeneratorTest : Jhu.Graywulf.Test.TestClassBase
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

        private Jhu.Graywulf.SqlParser.TableReference HtmInner
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

        private Jhu.Graywulf.SqlParser.TableReference HtmPartial
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

        [TestMethod]
        public void CreateRegionContainsConditionXyzTest()
        {
            var sql = @"
SELECT *
FROM table WITH (POINT(cx, cy, cz))";

            var ss = Parse(sql);
            var ts = ss.EnumerateSourceTables(false).First();

            var sc = CallMethod(CodeGenerator, "CreateRegionContainsCondition", ts);

            Assert.AreEqual("@r.ContainsXyz(cx, cy, cz) = 1", sc.ToString());
        }

        [TestMethod]
        public void CreateRegionContainsConditionEqTest()
        {
            var sql = @"
SELECT *
FROM table WITH (POINT(ra, dec))";

            var ss = Parse(sql);
            var ts = ss.EnumerateSourceTables(false).First();

            var sc = CallMethod(CodeGenerator, "CreateRegionContainsCondition", ts);

            Assert.AreEqual("@r.ContainsEq(ra, dec) = 1", sc.ToString());
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

            var ss = Parse(sql);
            var qs = ss.EnumerateQuerySpecifications().First();

            CallMethod(CodeGenerator, "AppendRegionJoinsAndConditions", qs, HtmTable, false);
            CallMethod(CodeGenerator, "RemoveNonStandardTokens", qs);

            Assert.AreEqual(gt, CodeGenerator.Execute(qs));
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

            var ss = Parse(sql);
            var qs = ss.EnumerateQuerySpecifications().First();

            CallMethod(CodeGenerator, "AppendRegionJoinsAndConditions", qs, HtmTable, true);
            CallMethod(CodeGenerator, "RemoveNonStandardTokens", qs);

            Assert.AreEqual(gt, CodeGenerator.Execute(qs));
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

            var ss = Parse(sql);
            var qs = ss.EnumerateQuerySpecifications().First();

            CallMethod(CodeGenerator, "AppendRegionJoinsAndConditions", qs, HtmTable, true);
            CallMethod(CodeGenerator, "RemoveNonStandardTokens", qs);

            Assert.AreEqual(gt, CodeGenerator.Execute(qs));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AppendRegionJoinsAndConditionsQuerySpecificationNoCoordinatesTest()
        {
            var sql = @"
SELECT table.*
FROM table WITH (HTMID(htmid))
REGION 'CIRCLE J2000 10 10 10'";

            var ss = Parse(sql);
            var qs = ss.EnumerateQuerySpecifications().First();

            CallMethod(CodeGenerator, "AppendRegionJoinsAndConditions", qs, HtmTable, true);
            CallMethod(CodeGenerator, "RemoveNonStandardTokens", qs);

            CodeGenerator.Execute(qs);
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
WHERE @r.ContainsEq(ra, dec) = 1
))";

            var ss = Parse(sql);
            var htminner = new[] { HtmInner };
            var htmpartial = new[] { HtmPartial };

            CallMethod(CodeGenerator, "AppendRegionJoinsAndConditions", ss, htminner, htmpartial);
            CallMethod(CodeGenerator, "RemoveNonStandardTokens", ss);

            Assert.AreEqual(gt, CodeGenerator.Execute(ss));
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
WHERE @r.ContainsEq(ra, dec) = 1
";

            var ss = Parse(sql);
            var htminner = new[] { HtmInner };
            var htmpartial = new[] { HtmPartial };

            CallMethod(CodeGenerator, "AppendRegionJoinsAndConditions", ss, htminner, htmpartial);
            CallMethod(CodeGenerator, "RemoveNonStandardTokens", ss);

            Assert.AreEqual(gt, CodeGenerator.Execute(ss));
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
WHERE @r.ContainsEq(ra, dec) = 1
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
WHERE @r.ContainsEq(ra, dec) = 1
))";

            var ss = Parse(sql);
            var htminner = new[] { HtmInner, HtmInner };
            var htmpartial = new[] { HtmPartial, HtmPartial };

            CallMethod(CodeGenerator, "AppendRegionJoinsAndConditions", ss, htminner, htmpartial);
            CallMethod(CodeGenerator, "RemoveNonStandardTokens", ss);

            Assert.AreEqual(gt, CodeGenerator.Execute(ss));
        }
    }
}
