using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Jobs.Query;
using Jhu.SkyQuery.Jobs.Query;

namespace Jhu.SkyQuery.Jobs.Query.Test
{
    [TestClass]
    public class XMatchQueryFactoryTest
    {
        private QueryBase CreateQuery(string query)
        {
            var f = new XMatchQueryFactory();
            return f.CreateQuery(query, ExecutionMode.SingleServer);
        }

        [TestMethod]
        public void TestSimpleQuery()
        {
            var q = CreateQuery("SELECT TOP 10 * FROM CatalogA");

            Assert.IsTrue(q is SqlQuery);
            Assert.IsFalse(q is XMatchQuery);
            Assert.IsFalse(q is BayesFactorXMatchQuery);
        }

        [TestMethod]
        public void TestXMatchQuery()
        {
            var q = CreateQuery(@"
SELECT * FROM
CatalogA a WITH(POINT(a.Ra, a.Dec)),
CatalogB b WITH(POINT(b.Ra, b.Dec))
XMATCH BAYESFACTOR AS x
MUST EXIST a
MUST EXIST b
HAVING LIMIT 1e3");

            Assert.IsTrue(q is SqlQuery);
            Assert.IsTrue(q is XMatchQuery);
            Assert.IsTrue(q is BayesFactorXMatchQuery);
        }
    }
}
