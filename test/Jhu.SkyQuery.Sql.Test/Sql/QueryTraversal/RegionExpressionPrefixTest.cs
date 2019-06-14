using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.SkyQuery.Sql.Parsing;
using Jhu.Graywulf.Sql.QueryTraversal;

namespace Jhu.SkyQuery.Sql.QueryTraversal
{
    [TestClass]
    public class RegionExpressionPrefixTest
    {
        private string Execute(string code)
        {
            var exp = new SkyQueryParser().Execute<RegionExpression>(code);
            return new SkyQueryTestVisitorSink().Execute(exp, ExpressionTraversalMethod.Prefix);
        }

        [TestMethod]
        public void OperatorsTest()
        {
            var res = Execute("CIRCLE(1,1,1) UNION CIRCLE(2,2,2)");
            Assert.AreEqual("UNION CIRCLE ( 1 , 1 , 1 ) CIRCLE ( 2 , 2 , 2 ) ", res);

            res = Execute("CIRCLE(1,1,1) UNION CIRCLE(2,2,2) UNION CIRCLE(3,3,3)");
            Assert.AreEqual("UNION UNION CIRCLE ( 1 , 1 , 1 ) CIRCLE ( 2 , 2 , 2 ) CIRCLE ( 3 , 3 , 3 ) ", res);

            res = Execute("CIRCLE(1,1,1) INTERSECT CIRCLE(2,2,2) UNION CIRCLE(3,3,3)");
            Assert.AreEqual("UNION INTERSECT CIRCLE ( 1 , 1 , 1 ) CIRCLE ( 2 , 2 , 2 ) CIRCLE ( 3 , 3 , 3 ) ", res);

            res = Execute("CIRCLE(1,1,1) UNION CIRCLE(2,2,2) INTERSECT CIRCLE(3,3,3)");
            Assert.AreEqual("UNION CIRCLE ( 1 , 1 , 1 ) INTERSECT CIRCLE ( 2 , 2 , 2 ) CIRCLE ( 3 , 3 , 3 ) ", res);

            res = Execute("CIRCLE(1,1,1) INTERSECT (CIRCLE(2,2,2) UNION CIRCLE(3,3,3))");
            Assert.AreEqual("INTERSECT CIRCLE ( 1 , 1 , 1 ) UNION CIRCLE ( 2 , 2 , 2 ) CIRCLE ( 3 , 3 , 3 ) ", res);

            res = Execute("CIRCLE(1,1,1) UNION CIRCLE(2,2,2) EXCEPT CIRCLE(3,3,3)");
            Assert.AreEqual("EXCEPT UNION CIRCLE ( 1 , 1 , 1 ) CIRCLE ( 2 , 2 , 2 ) CIRCLE ( 3 , 3 , 3 ) ", res);

            res = Execute("CIRCLE(1,1,1) EXCEPT CIRCLE(2,2,2) EXCEPT CIRCLE(3,3,3)");
            Assert.AreEqual("EXCEPT EXCEPT CIRCLE ( 1 , 1 , 1 ) CIRCLE ( 2 , 2 , 2 ) CIRCLE ( 3 , 3 , 3 ) ", res);

            res = Execute("CIRCLE(1,1,1) EXCEPT CIRCLE(2,2,2) INTERSECT CIRCLE(3,3,3)");
            Assert.AreEqual("EXCEPT CIRCLE ( 1 , 1 , 1 ) INTERSECT CIRCLE ( 2 , 2 , 2 ) CIRCLE ( 3 , 3 , 3 ) ", res);

            res = Execute("CIRCLE(1,1,1) INTERSECT (CIRCLE(2,2,2) EXCEPT CIRCLE(3,3,3))");
            Assert.AreEqual("INTERSECT CIRCLE ( 1 , 1 , 1 ) EXCEPT CIRCLE ( 2 , 2 , 2 ) CIRCLE ( 3 , 3 , 3 ) ", res);
        }

        [TestMethod]
        public void NotOperatorTest()
        {
            var res = Execute("CIRCLE(1,1,1) UNION NOT CIRCLE(2,2,2)");
            Assert.AreEqual("UNION CIRCLE ( 1 , 1 , 1 ) NOT CIRCLE ( 2 , 2 , 2 ) ", res);

            res = Execute("NOT CIRCLE(1,1,1) UNION CIRCLE(2,2,2)");
            Assert.AreEqual("UNION NOT CIRCLE ( 1 , 1 , 1 ) CIRCLE ( 2 , 2 , 2 ) ", res);

            res = Execute("NOT (CIRCLE(1,1,1) INTERSECT CIRCLE(2,2,2))");
            Assert.AreEqual("NOT INTERSECT CIRCLE ( 1 , 1 , 1 ) CIRCLE ( 2 , 2 , 2 ) ", res);
        }
    }
}
