using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Parsing;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.Graywulf.Sql.Validation;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Schema.SqlServer;

namespace Jhu.SkyQuery.Parser
{
    [TestClass]
    public class SkyQueryValidatorTest
    {
        private SchemaManager CreateSchemaManager()
        {
            return new SqlServerSchemaManager();
        }

        private XMatchSelectStatement Parse(string query)
        {
            // TODO: upgrade

            throw new NotImplementedException();

            /*
            var script = new SkyQueryParser().Execute<StatementBlock>(query);
            var statement = script.FindDescendantRecursive<Statement>();
            var select = statement.FindDescendant<XMatchSelectStatement>();
            var qs = select.QueryExpression.EnumerateQuerySpecifications().FirstOrDefault();

            var nr = new SkyQueryNameResolver();
            nr.DefaultTableDatasetName = Jhu.Graywulf.Test.Constants.TestDatasetName;
            nr.DefaultFunctionDatasetName = Jhu.Graywulf.Test.Constants.CodeDatasetName;
            nr.SchemaManager = CreateSchemaManager();
            nr.Execute(select);

            return select;
            */
        }

        private void Validate(SelectStatement ss)
        {
            // TODO: Use ScriptBlock

            throw new NotImplementedException();

            /*
            var v = new SkyQueryValidator();
            v.Execute(ss);
            */
        }

        [TestMethod]
        public void ValidXMatchQueryTest()
        {
            var sql =
@"SELECT a.ra, a.dec
FROM XMATCH
    (MUST EXIST IN CatalogA a WITH(POINT(a.ra, a.dec), ERROR(0.1)),
     MUST EXIST IN CatalogB b WITH (POINT(b.ra, b.dec), ERROR(0.2)),
     LIMIT BAYESFACTOR TO 1000) AS x";

            var ss = Parse(sql);
        }

        [TestMethod]
        [ExpectedException(typeof(ParserException))]
        public void XMatchUnionQueryTest()
        {
            var sql =
@"SELECT a.ra, a.dec
FROM XMATCH
    (MUST EXIST IN CatalogA a WITH(POINT(a.ra, a.dec), ERROR(0.1)),
     MUST EXIST IN CatalogB b WITH (POINT(b.ra, b.dec), ERROR(0.2)),
     LIMIT BAYESFACTOR TO 1000) AS x
UNION
SELECT a.ra, a.dec
FROM CatalogA a
";

            var ss = Parse(sql);

            Validate(ss);
        }

        [TestMethod]
        [ExpectedException(typeof(ParserException))]
        public void XMatchSubqueryTest()
        {
            var sql =
@"
SELECT * FROM
(
    SELECT a.ra, a.dec
    FROM XMATCH
        (MUST EXIST IN CatalogA a WITH(POINT(a.ra, a.dec), ERROR(0.1)),
         MUST EXIST IN CatalogB b WITH (POINT(b.ra, b.dec), ERROR(0.2)),
         LIMIT BAYESFACTOR TO 1000) AS x
) sq
";

            var ss = Parse(sql);

            Validate(ss);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidatorException))]
        public void CatalogWithNoPrimaryKeyTest()
        {
            var sql =
@"SELECT a.ra, a.dec
FROM XMATCH
    (MUST EXIST IN CatalogA a WITH(POINT(a.ra, a.dec), ERROR(0.1)),
     MUST EXIST IN [CatalogWithNoPrimaryKey] b WITH (POINT(b.ra, b.dec), ERROR(0.2)),
     LIMIT BAYESFACTOR TO 1000) AS x";

            var ss = Parse(sql);

            Validate(ss);
        }
    }
}
