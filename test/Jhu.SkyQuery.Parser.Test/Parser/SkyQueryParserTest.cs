using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.ParserLib;
using Jhu.SkyQuery.Parser;

namespace Jhu.SkyQuery.Parser.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class SkyQueryParserTest
    {
        protected QuerySpecification Parse(string query)
        {
            var p = new SkyQueryParser();
            return (QuerySpecification)((SelectStatement)p.Execute(query)).EnumerateQuerySpecifications().First();;
        }

        [TestMethod]
        public void XMatchTableHintTest()
        {
            var sql = "WITH(POINT(ra,dec), ERROR(1.0))";

            var p = new SkyQueryParser();
            var h = p.Execute(new XMatchHintClause(), sql);
        }

        [TestMethod]
        public void SimpleQueryTest()
        {
            var sql =
@"SELECT ra, dec FROM d:c";

            var qs = Parse(sql);
            Assert.IsNull(qs.FindDescendant<XMatchClause>());
        }

        [TestMethod]
        public void XMatchTableHintsTest()
        {
            var sql =
@"SELECT c1.ra, c1.dec, c2.ra, c2.dec
FROM d1:c1 WITH(POINT(c1.ra, c1.dec), ERROR(0.1)),
     d2:c2 WITH(POINT(c2.ra, c2.dec), ERROR(c2.err, 0.1, 0.5))
XMATCH BAYESFACTOR AS x
MUST EXIST c1
MUST EXIST c2
HAVING LIMIT 1e3
";

            var qs = Parse(sql);

            var ts = qs.EnumerateSourceTables(false).Cast<XMatchTableSource>().ToArray();

            Assert.AreEqual(2, ts.Length);

            Assert.AreEqual("POINT(c1.ra, c1.dec)", ts[0].Position.ToString());
            Assert.AreEqual("0.1", ts[0].ErrorExpression.ToString());
            Assert.IsTrue(ts[0].IsConstantError);

            Assert.AreEqual("POINT(c2.ra, c2.dec)", ts[1].Position.ToString());
            Assert.AreEqual("c2.err", ts[1].ErrorExpression.ToString());
            Assert.AreEqual("0.1", ts[1].MinErrorExpression.ToString());
            Assert.AreEqual("0.5", ts[1].MaxErrorExpression.ToString());
            Assert.IsFalse(ts[1].IsConstantError);
        }

    }
}
