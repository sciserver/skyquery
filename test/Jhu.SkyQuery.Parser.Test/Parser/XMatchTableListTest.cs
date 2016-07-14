using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.ParserLib;
using Jhu.SkyQuery.Parser;

namespace Jhu.SkyQuery.Parser
{
    [TestClass]
    public class XMatchTableListTest
    {
        protected XMatchTableList Parse(string query)
        {
            var p = new SkyQueryParser();
            var xt = new XMatchTableList();
            return (XMatchTableList)p.Execute(xt, query);
        }

        [TestMethod]
        public void SingleTableTest()
        {
            Parse("MUST EXIST IN SDSSDR7:PhotoObjAll");
            Parse("MAY EXIST IN SDSSDR7:PhotoObjAll");
            Parse("NOT EXIST IN SDSSDR7:PhotoObjAll");
            Parse("MUST EXIST IN SDSSDR7:PhotoObjAll a");
            Parse("MUST EXIST IN SDSSDR7:PhotoObjAll AS a");
            Parse("MUST EXIST IN SDSSDR7:PhotoObjAll a WITH(POINT(ra, dec))");
        }

        [TestMethod]
        public void MultipleTablesTest()
        {
            Parse(@"MUST EXIST IN SDSSDR7:PhotoObjAll a WITH(POINT(ra, dec)),
                    MUST EXIST IN d2:c2 WITH(POINT(c2.ra, c2.dec), ERROR(c2.err, 0.1, 0.5))");
        }
        
        [TestMethod]
        public void XMatchCoordinateHintsTest()
        {
            var sql =
@"SELECT c1.ra, c1.dec, c2.ra, c2.dec
FROM 
    XMATCH x AS
    (
        MUST EXIST IN d1:c1 WITH(POINT(c1.ra, c1.dec), ERROR(0.1)),
        MUST EXIST IN d2:c2 WITH(POINT(c2.ra, c2.dec), ERROR(c2.err, 0.1, 0.5))
        LIMIT BAYESFACTOR TO 1000
    )
";
            /*
            var qs = Parse(sql);

            var ts = qs.EnumerateSourceTables(false).Cast<CoordinateTableSource>().ToArray();

            Assert.AreEqual(2, ts.Length);

            Assert.AreEqual("POINT(c1.ra, c1.dec)", ts[0].Position.ToString());
            Assert.AreEqual("0.1", ts[0].ErrorExpression.ToString());
            Assert.IsTrue(ts[0].IsConstantError);

            Assert.AreEqual("POINT(c2.ra, c2.dec)", ts[1].Position.ToString());
            Assert.AreEqual("c2.err", ts[1].ErrorExpression.ToString());
            Assert.AreEqual("0.1", ts[1].MinErrorExpression.ToString());
            Assert.AreEqual("0.5", ts[1].MaxErrorExpression.ToString());
            Assert.IsFalse(ts[1].IsConstantError);*/
        }

    }
}
