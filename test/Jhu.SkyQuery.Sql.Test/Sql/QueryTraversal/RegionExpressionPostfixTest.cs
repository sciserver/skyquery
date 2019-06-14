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
    public class RegionExpressionPostfixTest
    {
        private string Execute(string code)
        {
            var exp = new SkyQueryParser().Execute<RegionExpression>(code);
            return new SkyQueryTestVisitorSink().Execute(exp, ExpressionTraversalMethod.Postfix);
        }

        [TestMethod]
        public void OperatorsTest()
        {
            var res = Execute("CIRCLE(1,1,1) UNION CIRCLE(2,2,2)");
            Assert.AreEqual("CIRCLE ( 1 , 1 , 1 ) CIRCLE ( 2 , 2 , 2 ) UNION ", res);

            res = Execute("CIRCLE(1,1,1) UNION CIRCLE(2,2,2) UNION CIRCLE(3,3,3)");
            Assert.AreEqual("CIRCLE ( 1 , 1 , 1 ) CIRCLE ( 2 , 2 , 2 ) CIRCLE ( 3 , 3 , 3 ) UNION UNION ", res);

            res = Execute("CIRCLE(1,1,1) INTERSECT CIRCLE(2,2,2) UNION CIRCLE(3,3,3)");
            Assert.AreEqual("CIRCLE ( 1 , 1 , 1 ) CIRCLE ( 2 , 2 , 2 ) INTERSECT CIRCLE ( 3 , 3 , 3 ) UNION ", res);

            res = Execute("CIRCLE(1,1,1) UNION CIRCLE(2,2,2) INTERSECT CIRCLE(3,3,3)");
            Assert.AreEqual("CIRCLE ( 1 , 1 , 1 ) CIRCLE ( 2 , 2 , 2 ) CIRCLE ( 3 , 3 , 3 ) INTERSECT UNION ", res);

            res = Execute("CIRCLE(1,1,1) INTERSECT (CIRCLE(2,2,2) UNION CIRCLE(3,3,3))");
            Assert.AreEqual("CIRCLE ( 1 , 1 , 1 ) CIRCLE ( 2 , 2 , 2 ) CIRCLE ( 3 , 3 , 3 ) UNION INTERSECT ", res);

            res = Execute("CIRCLE(1,1,1) UNION CIRCLE(2,2,2) EXCEPT CIRCLE(3,3,3)");
            Assert.AreEqual("CIRCLE ( 1 , 1 , 1 ) CIRCLE ( 2 , 2 , 2 ) CIRCLE ( 3 , 3 , 3 ) EXCEPT UNION ", res);

            res = Execute("CIRCLE(1,1,1) EXCEPT CIRCLE(2,2,2) EXCEPT CIRCLE(3,3,3)");
            Assert.AreEqual("CIRCLE ( 1 , 1 , 1 ) CIRCLE ( 2 , 2 , 2 ) CIRCLE ( 3 , 3 , 3 ) EXCEPT EXCEPT ", res);

            res = Execute("CIRCLE(1,1,1) EXCEPT CIRCLE(2,2,2) INTERSECT CIRCLE(3,3,3)");
            Assert.AreEqual("CIRCLE ( 1 , 1 , 1 ) CIRCLE ( 2 , 2 , 2 ) CIRCLE ( 3 , 3 , 3 ) INTERSECT EXCEPT ", res);

            res = Execute("CIRCLE(1,1,1) INTERSECT (CIRCLE(2,2,2) EXCEPT CIRCLE(3,3,3))");
            Assert.AreEqual("CIRCLE ( 1 , 1 , 1 ) CIRCLE ( 2 , 2 , 2 ) CIRCLE ( 3 , 3 , 3 ) EXCEPT INTERSECT ", res);
        }

        [TestMethod]
        public void NotOperatorTest()
        {
            var res = Execute("CIRCLE(1,1,1) UNION NOT CIRCLE(2,2,2)");
            Assert.AreEqual("CIRCLE ( 1 , 1 , 1 ) CIRCLE ( 2 , 2 , 2 ) NOT UNION ", res);

            res = Execute("NOT CIRCLE(1,1,1) UNION CIRCLE(2,2,2)");
            Assert.AreEqual("CIRCLE ( 1 , 1 , 1 ) NOT CIRCLE ( 2 , 2 , 2 ) UNION ", res);

            res = Execute("NOT (CIRCLE(1,1,1) INTERSECT CIRCLE(2,2,2))");
            Assert.AreEqual("CIRCLE ( 1 , 1 , 1 ) CIRCLE ( 2 , 2 , 2 ) INTERSECT NOT ", res);
        }
    }
}
