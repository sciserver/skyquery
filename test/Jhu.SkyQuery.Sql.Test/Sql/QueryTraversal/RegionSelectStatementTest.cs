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
    public class RegionSelectStatementTest : SkyQueryTestBase
    {
        private string Execute(string code)
        {
            var exp = Parse(code);
            return new SkyQueryTestVisitorSink().Execute(exp);
        }

        [TestMethod]
        public void RegionQueryTest()
        {
            var sql =
@"SELECT DISTINCT TOP 10 PERCENT ID AS Col1, Col2 = Data, AVG(Data2)
INTO outtable
FROM Tab1 AS t WITH(TABLOCKX)
INNER LOOP JOIN Tab2 t2 WITH(TABLOCK) ON t.ID = t2.ID
WHERE ID IN (1, 2, 4)
REGION CIRCLE(10, 10, 20)
GROUP BY ID, Data
HAVING AVG(Data2) > 10
";

            var gt = "FROM Tab1 AS t WITH ( TABLOCKX ) INNER LOOP JOIN Tab2 t2 WITH ( TABLOCK ) ON t . ID = t2 . ID WHERE ID IN ( 1 , 2 , 4 ) GROUP BY ID , Data HAVING AVG ( Data2 ) > 10 SELECT DISTINCT TOP 10 PERCENT ID AS Col1 , Col2 = Data , AVG ( Data2 ) INTO outtable REGION CIRCLE ( 10 , 10 , 20 ) ";

            var res = Execute(sql);
            Assert.AreEqual(gt, res);
        }

        [TestMethod]
        public void RegionQueryWithUnionTest()
        {
            var sql =
@"SELECT DISTINCT TOP 10 PERCENT ID AS Col1, Col2 = Data, AVG(Data2)
INTO outtable
FROM Tab1 AS t WITH(TABLOCKX)
INNER LOOP JOIN Tab2 t2 WITH(TABLOCK) ON t.ID = t2.ID
WHERE ID IN (1, 2, 4)
REGION CIRCLE(10, 10, 20)
GROUP BY ID, Data
HAVING AVG(Data2) > 10
UNION ALL
SELECT DISTINCT ID, Data FROM Tab2";

            var gt = "FROM Tab1 AS t WITH ( TABLOCKX ) INNER LOOP JOIN Tab2 t2 WITH ( TABLOCK ) ON t . ID = t2 . ID WHERE ID IN ( 1 , 2 , 4 ) GROUP BY ID , Data HAVING AVG ( Data2 ) > 10 SELECT DISTINCT TOP 10 PERCENT ID AS Col1 , Col2 = Data , AVG ( Data2 ) INTO outtable REGION CIRCLE ( 10 , 10 , 20 ) UNION ALL FROM Tab2 SELECT DISTINCT ID , Data ";

            var res = Execute(sql);
            Assert.AreEqual(gt, res);
        }
    }
}
