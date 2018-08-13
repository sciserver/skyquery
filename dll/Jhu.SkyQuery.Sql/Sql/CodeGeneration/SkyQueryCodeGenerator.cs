using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Graywulf.Sql.Schema;
using Jhu.Graywulf.Sql.QueryGeneration;
using Jhu.Graywulf.Sql.QueryGeneration.SqlServer;
using Jhu.Graywulf.Sql.NameResolution;
using Jhu.SkyQuery.Sql.Parsing;

namespace Jhu.SkyQuery.Sql.CodeGeneration
{
    public class SkyQueryCodeGenerator : SqlServerQueryGenerator
    {
        public override ColumnListGeneratorBase CreateColumnListGenerator()
        {
            return new SkyQueryColumnListGenerator();
        }

        public static new string QuoteIdentifier(string identifier)
        {
            return SqlServerQueryGenerator.QuoteIdentifier(identifier);
        }

        public string GenerateXMatchQuery(XMatch xmatch, SchemaManager schemaManager)
        {
            var columnlist = GenerateXMatchSelectColumnList(xmatch, schemaManager);
            var tablelist = GenerateXMatchCatalogTableList(xmatch, schemaManager);
            var where = GenerateXMatchWhereClause(xmatch, schemaManager);

            var sql = new StringBuilder();
            sql.AppendFormat(@"SELECT {0}
INTO {1}:{2}
FROM XMATCH (
{3}    LIMIT BAYESFACTOR TO {4}) AS {5}
",
                columnlist,
                xmatch.TargetDataset,
                xmatch.TargetTable,
                tablelist,
                xmatch.BayesFactor.ToString(System.Globalization.CultureInfo.InvariantCulture),
                Constants.XMatchDefaultAlias);

            if (!String.IsNullOrWhiteSpace(where))
            {
                sql.AppendLine(where);
            }

            if (!String.IsNullOrWhiteSpace(xmatch.Region))
            {
                sql.Append("REGION '");
                sql.Append(xmatch.Region);
                sql.Append("'");
            }

            return sql.ToString();
        }

        private string GenerateXMatchSelectColumnList(XMatch xmatch, SchemaManager schemaManager)
        {
            var columns = new List<ColumnReference>();

            // Add xmatch columns
            {
                var tr = new NameResolution.BayesFactorXMatchTableReference()
                {
                    Alias = Constants.XMatchDefaultAlias
                };

                foreach (var cr in tr.ColumnReferences)
                {
                    if (xmatch.Columns.Contains(cr.ColumnName))
                    {
                        columns.Add(cr);
                    }
                }
            }

            // Add catalog columns
            foreach (var catalog in xmatch.Catalogs)
            {
                var ds = schemaManager.Datasets[catalog.DatasetName];
                var t = (TableOrView)ds.GetObject(catalog.TableUniqueKey);
                var tr = new TableReference(t, catalog.Alias, false);

                foreach (var column in catalog.Columns)
                {
                    var c = t.Columns[column];
                    var cr = new ColumnReference(c, tr, new DataTypeReference(c.DataType));
                    columns.Add(cr);
                }
            }

            var cl = new SkyQueryColumnListGenerator(columns)
            {
                ListType = ColumnListType.SelectWithOriginalNameNoAlias
            };

            return cl.Execute();
        }

        private string GenerateXMatchCatalogTableList(XMatch xmatch, SchemaManager schemaManager)
        {
            var tables = new StringBuilder();

            foreach (var catalog in xmatch.Catalogs)
            {
                tables.Append(GenerateXMatchCatalogTableSource(catalog, schemaManager));
                tables.AppendLine(",");
            }

            return tables.ToString();
        }

        private string GenerateXMatchCatalogTableSource(Catalog catalog, SchemaManager schemaManager)
        {
            var cg = new SqlServerQueryGenerator();
            var ds = schemaManager.Datasets[catalog.DatasetName];
            var table = (TableOrView)ds.GetObject(catalog.TableUniqueKey);
            string with = "";

            var sql = "    MUST EXIST IN {0}:{1}.{2} AS {3} {4}";

            switch (catalog.CoordinateMode)
            {
                case CoordinateMode.Automatic:
                    break;
                case CoordinateMode.Manual:
                    with += String.Format(
                        ", POINT({0}, {1})",
                        SqlServerQueryGenerator.QuoteIdentifier(catalog.RaColumn),
                        SqlServerQueryGenerator.QuoteIdentifier(catalog.DecColumn));
                    break;
                default:
                    throw new NotImplementedException();
            }

            switch (catalog.ErrorMode)
            {
                case ErrorMode.Constant:
                    with += String.Format(
                        ", ERROR({0})",
                        catalog.ErrorValue.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    break;
                case ErrorMode.Column:
                    with += String.Format(
                        ", ERROR({0}, {1}, {2})",
                        SqlServerQueryGenerator.QuoteIdentifier(catalog.ErrorColumn),
                        catalog.ErrorMin.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        catalog.ErrorMax.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (!String.IsNullOrWhiteSpace(with))
            {
                with = String.Format("WITH({0})", with.Substring(2));
            }

            sql = String.Format(sql,
                SqlServerQueryGenerator.QuoteIdentifier(ds.Name),
                SqlServerQueryGenerator.QuoteIdentifier(table.SchemaName),
                SqlServerQueryGenerator.QuoteIdentifier(table.ObjectName),
                SqlServerQueryGenerator.QuoteIdentifier(catalog.Alias),
                with);

            return sql;
        }

        private string GenerateXMatchWhereClause(XMatch xmatch, SchemaManager schemaManager)
        {
            var sql = new StringBuilder();
            var q = 0;

            foreach (var catalog in xmatch.Catalogs)
            {
                if (String.IsNullOrWhiteSpace(catalog.Where))
                {
                    continue;
                }

                if (q == 0)
                {
                    sql.Append("WHERE ");
                }
                else
                {
                    sql.AppendLine(" AND");
                }

                sql.Append("(");
                sql.Append(catalog.Where);
                sql.Append(")");

                q++;
            }

            return sql.ToString();
        }
    }
}
