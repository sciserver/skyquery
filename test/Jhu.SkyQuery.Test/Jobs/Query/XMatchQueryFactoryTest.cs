using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Scheduler;
using Jhu.Graywulf.RemoteService;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Jobs.Query;
using Jhu.SkyQuery.Jobs.Query;

namespace Jhu.SkyQuery.Jobs.Query.Test
{
    [TestClass]
    public class XMatchQueryFactoryTest : SkyQueryTestBase
    {
        private SqlQuery CreateQuery(string query)
        {
            var f = new SingleServerQueryFactory();
            return f.CreateQuery(query);
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
        public void TestRegionQuery()
        {
            var q = CreateQuery(@"
SELECT * 
FROM CatalogA a WITH(POINT(a.Ra, a.Dec))
REGION 'CIRCLE J2000 10 10 10'");

            Assert.IsTrue(q is SqlQuery);
            Assert.IsTrue(q is RegionQuery);
            Assert.IsFalse(q is XMatchQuery);
        }

        [TestMethod]
        public void TestXMatchQuery()
        {
            var q = CreateQuery(@"
SELECT * FROM
    XMATCH
    (MUST EXIST IN CatalogA a WITH(POINT(a.Ra, a.Dec)),
     MUST EXIST IN CatalogB b WITH(POINT(b.Ra, b.Dec)),
     LIMIT BAYESFACTOR TO 1e3) AS x");

            Assert.IsTrue(q is SqlQuery);
            Assert.IsTrue(q is XMatchQuery);
            Assert.IsTrue(q is BayesFactorXMatchQuery);
        }
    }
}
