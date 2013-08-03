using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Jobs.Query;
using Jhu.SkyQuery.Jobs.Query;

namespace Jhu.SkyQuery.Test
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
            var q = CreateQuery("SELECT * FROM CatalogA a, CatalogB b XMATCH BAYESFACTOR AS x MUST EXIST a ON POINT(a.Ra, a.Dec) MUST EXIST b ON POINT(b.Ra, b.Dec) HAVING LIMIT 1e3");

            Assert.IsTrue(q is SqlQuery);
            Assert.IsTrue(q is XMatchQuery);
            Assert.IsTrue(q is BayesFactorXMatchQuery);
        }
    }
}
