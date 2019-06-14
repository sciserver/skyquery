using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Sql.Schema;

namespace Jhu.SkyQuery.Sql.CodeGeneration
{
    [TestClass]
    public class SkyQueryCodeGeneratorTest : SkyQueryTestBase
    {
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            StartLogger();
        }

        [ClassCleanup]
        public static void CleanUp()
        {
            StopLogger();
        }

        [TestMethod]
        public void GenerateXMatchWhereClauseTest()
        {
            var xmatch = new XMatch()
            {
                TargetDataset = "MYDB",
                TargetTable = "webuser.xmatchtest",
                BayesFactor = 1e3,
                Region = "CIRCLE J2000 10 10 10",
            };
            xmatch.Columns.AddRange(new[] { "matchid", "ra", "dec" });

            var c1 = new Catalog()
            {
                ID = 0,
                InclusionMethod = Parsing.XMatchInclusionMethod.Must,
                DatasetName = "SDSSDR13",
                TableUniqueKey = "Table|SDSSDR13|SkyNode_SDSSDR13|dbo|PhotoObjAll",     // TODO
                Alias = "s",
                CoordinateMode = CoordinateMode.Automatic,
                ErrorMode = ErrorMode.Constant,
                ErrorValue = 0.1,
                Where = "ra BETWEEN 0 AND 10"
            };
            c1.Columns.AddRange(new[] { "objID", "ra", "dec" });
            xmatch.Catalogs.Add(c1);

            var c2 = new Catalog()
            {
                ID = 0,
                InclusionMethod = Parsing.XMatchInclusionMethod.Must,
                DatasetName = "TwoMASS",
                TableUniqueKey = "Table|TwoMASS|SkyNode_TwoMASS|dbo|PhotoPSC",    // TODO
                Alias = "t",
                CoordinateMode = CoordinateMode.Automatic,
                ErrorMode = ErrorMode.Constant,
                ErrorValue = 0.1,
                Where = "ra BETWEEN 0 AND 10"
            };
            c2.Columns.AddRange(new[] { "objID", "ra", "dec" });
            xmatch.Catalogs.Add(c2);

            var cg = new SkyQueryCodeGenerator();
            var q = cg.GenerateXMatchQuery(xmatch, SchemaManager);

            var gt =
@"SELECT [x].[MatchID], [x].[RA], [x].[Dec], [s].[objID], [s].[ra], [s].[dec], [t].[objID], [t].[ra], [t].[dec]
INTO MYDB:webuser.xmatchtest
FROM XMATCH (
    MUST EXIST IN [SDSSDR13]:[dbo].[PhotoObjAll] AS [s] WITH(ERROR(0.1)),
    MUST EXIST IN [TwoMASS]:[dbo].[PhotoPSC] AS [t] WITH(ERROR(0.1)),
    LIMIT BAYESFACTOR TO 1000) AS x
WHERE (ra BETWEEN 0 AND 10) AND
(ra BETWEEN 0 AND 10)
REGION 'CIRCLE J2000 10 10 10'";

            Assert.AreEqual(gt, q);
        }
    }
}
