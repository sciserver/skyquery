using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.SqlCodeGen;
using Jhu.Graywulf.SqlCodeGen.SqlServer;
using Jhu.Graywulf.SqlParser;
using Jhu.SkyQuery.Parser;

namespace Jhu.SkyQuery.CodeGen
{
    public class SkyQueryCodeGenerator : SqlServerCodeGenerator
    {
        private SchemaManager schemaManager;
        private SkyQueryParser parser;
        private SkyQueryNameResolver nameResolver;
        private DatasetBase defaultTableDataset;
        private DatasetBase defaultFunctionDataset;

        public SchemaManager SchemaManager
        {
            get { return schemaManager; }
            set { schemaManager = value; }
        }

        public DatasetBase DefaultTableDataset
        {
            get { return defaultTableDataset; }
            set { defaultTableDataset = value; }
        }

        public DatasetBase DefaultFunctionDataset
        {
            get { return defaultFunctionDataset; }
            set { defaultFunctionDataset = value; }
        }

        public SkyQueryCodeGenerator(SchemaManager schemaManager)
        {
            InitializeMembers();

            this.schemaManager = schemaManager;
            this.parser = new SkyQueryParser();
            this.nameResolver = new SkyQueryNameResolver()
            {
                SchemaManager = schemaManager
            };
        }

        private void InitializeMembers()
        {
            this.schemaManager = null;
            this.parser = null;
            this.nameResolver = null;
            this.defaultTableDataset = null;
            this.defaultFunctionDataset = null;
        }

        private SkyQueryParser CreateParser()
        {
            return new SkyQueryParser();
        }

        private SqlNameResolver CreateNameResolver()
        {
            var nr = new SkyQueryNameResolver();
            nr.SchemaManager = schemaManager;
            nr.DefaultTableDatasetName = defaultTableDataset?.Name;
            nr.DefaultFunctionDatasetName = defaultFunctionDataset?.Name;

            return nr;
        }

        public override SqlColumnListGeneratorBase CreateColumnListGenerator(TableReference table, ColumnContext columnContext, ColumnListType listType)
        {
            return new SkyQueryColumnListGenerator(table, columnContext, listType);
        }

        public static new string QuoteIdentifier(string identifier)
        {
            return SqlServerCodeGenerator.QuoteIdentifier(identifier);
        }

        public string GenerateXMatchQuery(XMatch xmatch)
        {
            ResolveReferences(xmatch);

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

        private void ResolveReferences(XMatch xmatch)
        {
            foreach (var catalog in xmatch.Catalogs)
            {
                ResolveCatalogTableReference(catalog);
                ResolveCatalogWhereCondition(catalog);
            }
        }

        private void ResolveCatalogTableReference(Catalog catalog)
        {
            var name = (TableOrViewName)parser.Execute(new TableOrViewName(), catalog.TableOrViewName);
            var tr = new TableReference(name);
            tr.SubstituteDefaults(schemaManager, catalog.DatasetName);
            tr.Alias = catalog.Alias;
            catalog.TableReference = nameResolver.ResolveSourceTableReference(tr);
        }

        private void ResolveCatalogWhereCondition(Catalog catalog)
        {
            if (!String.IsNullOrWhiteSpace(catalog.Where))
            {
                catalog.SearchCondition = (SkyQuery.Parser.SearchCondition)parser.Execute(new SkyQuery.Parser.SearchCondition(), catalog.Where);

                foreach (var ci in catalog.SearchCondition.EnumerateDescendantsRecursive<ColumnIdentifier>())
                {
                    ci.TableReference = new TableReference(catalog.TableReference.TableOrView, catalog.Alias, false);
                }
            }
            else
            {
                catalog.SearchCondition = null;
            }
        }

        private string GenerateXMatchSelectColumnList(XMatch xmatch, SchemaManager schemaManager)
        {
            var columns = new List<ColumnReference>();

            // Add xmatch columns
            {
                var tr = new BayesFactorXMatchTableReference()
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
                foreach (var column in catalog.Columns)
                {
                    var cr = new ColumnReference(catalog.TableReference, catalog.TableReference.TableOrView.Columns[column]);
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
            var cg = new SqlServerCodeGenerator();
            var ds = schemaManager.Datasets[catalog.DatasetName];
            var table = catalog.TableReference.TableOrView;
            string with = "";

            var sql = "    MUST EXIST IN {0}:{1}.{2} AS {3} {4}";

            switch (catalog.CoordinateMode)
            {
                case CoordinateMode.Automatic:
                    break;
                case CoordinateMode.Manual:
                    with += String.Format(
                        ", POINT({0}, {1})",
                        SqlServerCodeGenerator.QuoteIdentifier(catalog.RaColumn),
                        SqlServerCodeGenerator.QuoteIdentifier(catalog.DecColumn));
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
                        SqlServerCodeGenerator.QuoteIdentifier(catalog.ErrorColumn),
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
                SqlServerCodeGenerator.QuoteIdentifier(ds.Name),
                SqlServerCodeGenerator.QuoteIdentifier(table.SchemaName),
                SqlServerCodeGenerator.QuoteIdentifier(table.ObjectName),
                SqlServerCodeGenerator.QuoteIdentifier(catalog.Alias),
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
                    sql.AppendLine("WHERE");
                }
                else
                {
                    sql.AppendLine(" AND");
                }

                sql.Append("    (");
                sql.Append(GetCode(catalog.SearchCondition, true));
                sql.Append(")");

                q++;
            }

            return sql.ToString();
        }

        private string GenerateCatalogWhereCondition(Catalog catalog)
        {
            // Prefix each column with alias if nothing is specified
            var p = CreateParser();
            var sc = p.Execute(new SkyQuery.Parser.SearchCondition(), catalog.Where);

            SubstituteCatalogTableAlias((Jhu.Graywulf.ParserLib.Node)sc, catalog);

            return GetCode((Jhu.Graywulf.ParserLib.Node)sc, true);
        }

        private void SubstituteCatalogTableAlias(Jhu.Graywulf.ParserLib.Node node, Catalog catalog)
        {
            if (node is ITableReference)
            {
            }

            foreach (var n in node.Stack)
            {
                if (n is Jhu.Graywulf.ParserLib.Node)
                {
                    SubstituteCatalogTableAlias((Jhu.Graywulf.ParserLib.Node)n, catalog);
                }
            }
        }

        public Catalog CreateCatalog(string datasetName, string tableOrViewName)
        {
            var catalog = new Catalog()
            {
                DatasetName = datasetName,
                TableOrViewName = tableOrViewName,
            };

            ResolveCatalogTableReference(catalog);

            if (catalog.TableReference.TableOrView.PrimaryKey != null)
            {
                foreach (var ic in catalog.TableReference.TableOrView.PrimaryKey.Columns.Values)
                {
                    catalog.Columns.Add(ic.Name);
                }
            }

            return catalog;
        }
    }
}
