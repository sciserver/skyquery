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
        public void ValidXMatchQueryTest()
        {
            var sql =
@"SELECT a.ra, a.dec
FROM XMATCH x AS
    (MUST EXIST IN CatalogA a WITH(POINT(a.ra, a.dec), ERROR(0.1)),
     MUST EXIST IN CatalogB b WITH (POINT(b.ra, b.dec), ERROR(0.2))
     LIMIT BAYESFACTOR TO 1000)
";

            var ss = Parse(sql);
        }

        [TestMethod]
        public void XMatchUnionQueryTest()
        {
            var sql =
@"SELECT a.ra, a.dec
FROM XMATCH x AS
    (MUST EXIST IN CatalogA a WITH(POINT(a.ra, a.dec), ERROR(0.1)),
     MUST EXIST IN CatalogB b WITH (POINT(b.ra, b.dec), ERROR(0.2))
     LIMIT BAYESFACTOR TO 1000)
UNION
SELECT a.ra, a.dec
FROM CatalogA a
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

        [TestMethod]
        public void XMatchSubqueryTest()
        {
            var sql =
@"
SELECT * FROM
(
    SELECT a.ra, a.dec
    FROM XMATCH x AS
        (MUST EXIST IN CatalogA a WITH(POINT(a.ra, a.dec), ERROR(0.1)),
         MUST EXIST IN CatalogB b WITH (POINT(b.ra, b.dec), ERROR(0.2))
         LIMIT BAYESFACTOR TO 1000)
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

        [TestMethod]
        public void CatalogWithNoPrimaryKeyTest()
        {
            var sql =
@"SELECT a.ra, a.dec
FROM XMATCH x AS
    (MUST EXIST IN CatalogA a WITH(POINT(a.ra, a.dec), ERROR(0.1)),
     MUST EXIST IN [CatalogWithNoPrimaryKey] b WITH (POINT(b.ra, b.dec), ERROR(0.2))
     LIMIT BAYESFACTOR TO 1000)";

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
