using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.Graywulf.Sql.QueryTraversal;

namespace Jhu.SkyQuery.Sql.QueryTraversal
{
    [TestClass]
    public class XMatchSelectStatementTest : SkyQueryTestBase
    {
        private string Execute(string code)
        {
            var exp = Parse(code);
            return new SkyQueryTestVisitorSink().Execute(exp);
        }

        [TestMethod]
        public void SimpleXMatchQueryTest()
        {
            var sql =
@"SELECT x.ra, x.dec, a.objID, b.objID
FROM XMATCH
    (
        MUST EXIST IN CATALOGA:PhotoObj AS a WITH (POINT(ra, dec)),
        MUST EXIST IN CATALOGB:SpecObj AS b WITH (POINT(ra, dec), ERROR(radecerr)),
        LIMIT BAYESFACTOR TO 1e3
    ) AS x
WHERE a.mask IN (1, 2, 4)
REGION CIRCLE(10, 10, 20)";

            var gt = "FROM XMATCH ( MUST EXIST IN CATALOGA:PhotoObj AS a WITH ( POINT ( ra , dec ) ) , MUST EXIST IN CATALOGB:SpecObj AS b WITH ( POINT ( ra , dec ) , ERROR ( radecerr ) ) , LIMIT BAYESFACTOR TO 1e3 ) AS x WHERE a . mask IN ( 1 , 2 , 4 ) SELECT x . ra , x . dec , a . objID , b . objID REGION CIRCLE ( 10 , 10 , 20 ) ";

            var res = Execute(sql);
            Assert.AreEqual(gt, res);
        }

        [TestMethod]
        public void SimpleXMatchQueryTest2()
        {
            var sql =
@"SELECT x.ra, x.dec, a.objID, b.objID
FROM x AS XMATCH
    (
        MUST EXIST IN CATALOGA:PhotoObj AS a WITH (POINT(ra, dec)),
        MUST EXIST IN CATALOGB:SpecObj AS b WITH (POINT(ra, dec), ERROR(radecerr)),
        LIMIT BAYESFACTOR TO 1e3
    )
WHERE a.mask IN (1, 2, 4)
REGION CIRCLE(10, 10, 20)";

            var gt = "FROM x AS XMATCH ( MUST EXIST IN CATALOGA:PhotoObj AS a WITH ( POINT ( ra , dec ) ) , MUST EXIST IN CATALOGB:SpecObj AS b WITH ( POINT ( ra , dec ) , ERROR ( radecerr ) ) , LIMIT BAYESFACTOR TO 1e3 ) WHERE a . mask IN ( 1 , 2 , 4 ) SELECT x . ra , x . dec , a . objID , b . objID REGION CIRCLE ( 10 , 10 , 20 ) ";

            var res = Execute(sql);
            Assert.AreEqual(gt, res);
        }
    }
}
