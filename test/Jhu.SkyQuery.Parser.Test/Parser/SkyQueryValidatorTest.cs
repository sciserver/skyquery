using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;
using Jhu.SkyQuery.Parser;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Schema.SqlServer;

namespace Jhu.SkyQuery.Parser.Test
{
    [TestClass]
    public class SkyQueryValidatorTest
    {
        private SchemaManager CreateSchemaManager()
        {
            return new SqlServerSchemaManager();
        }

        private SelectStatement Parse(string query)
        {
            var p = new SkyQueryParser();
            var ss = (SelectStatement)p.Execute(query);

            var qs = (SkyQuery.Parser.QuerySpecification)ss.EnumerateQuerySpecifications().First();

            var nr = new SkyQueryNameResolver();
            nr.DefaultTableDatasetName = Jhu.Graywulf.Test.Constants.TestDatasetName;
            nr.DefaultFunctionDatasetName = Jhu.Graywulf.Test.Constants.CodeDatasetName;
            nr.SchemaManager = CreateSchemaManager();
            nr.Execute(ss);

            return ss;
        }

        private void Validate(SelectStatement ss)
        {
            var v = new SkyQueryValidator();
            v.Execute(ss);
        }

        [TestMethod]
        public void SubqueryInXMatchTest()
        {
            var sql =
@"SELECT *
FROM CatalogA a WITH(POINT(a.ra, a.dec), ERROR(0.1)),
(SELECT * FROM CatalogB) b --WITH(POINT(b.ra, b.dec), ERROR(0.2))
XMATCH BAYESFACTOR x
MUST EXIST a
MUST EXIST b
HAVING LIMIT 1e3
";

            var ss = Parse(sql);

            try
            {
                Validate(ss);
                Assert.Fail();
            }
            catch (ValidatorException)
            {
            }
        }

        // TODO: write test to validate xmatch functions

        [TestMethod]
        public void XMatchSubqueryTest()
        {
            var sql =
@"
SELECT * FROM
(
    SELECT a.ra, a.dec
    FROM CatalogA a WITH(POINT(a.ra, a.dec), ERROR(0.1)),
        (SELECT * FROM CatalogB) b --WITH (POINT(b.ra, b.dec), ERROR(0.2))
    XMATCH BAYESFACTOR x
    MUST EXIST a
    MUST EXIST b
    HAVING LIMIT 1e3
) sq
";

            var ss = Parse(sql);

            try
            {
                Validate(ss);
                Assert.Fail();
            }
            catch (ValidatorException ex)
            {
            }
        }
    }
}
