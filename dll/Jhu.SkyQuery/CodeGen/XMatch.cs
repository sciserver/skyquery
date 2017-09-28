using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Graywulf.Schema;

namespace Jhu.SkyQuery.CodeGen
{
    public class XMatch
    {
        private string targetDataset;
        private string targetTable;
        private double bayesFactor;
        private string region;
        private List<string> columns;
        private List<Catalog> catalogs;

        public string TargetDataset
        {
            get { return targetDataset; }
            set { targetDataset = value; }
        }

        public string TargetTable
        {
            get { return targetTable; }
            set { targetTable = value; }
        }

        /// <summary>
        /// TODO: inherit and move this to subclass
        /// </summary>
        public double BayesFactor
        {
            get { return bayesFactor; }
            set { bayesFactor = value; }
        }

        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        public List<string> Columns
        {
            get { return columns; }
        }

        public List<Catalog> Catalogs
        {
            get { return catalogs; }
        }

        public XMatch()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.targetDataset = Jhu.Graywulf.Registry.Constants.UserDbName;
            this.targetTable = "xmatchtable";
            this.bayesFactor = 1e3;
            this.region = "CIRCLE J2000 0.0 0.0 10.0";
            this.columns = new List<string>()
            {
                "MatchID", "RA", "Dec"
            };
            this.catalogs = new List<Catalog>();
        }

        public void AddCatalog(Catalog catalog)
        {
            var alias = GenerateCatalogAlias(catalog.TableReference.TableOrView);
            catalog.Alias = alias;
            catalogs.Add(catalog);
        }

        public string GenerateCatalogAlias(TableOrView table)
        {
            var c = 0;
            var a = table.DatasetName.Substring(0, 1).ToLowerInvariant();
            var alias = a;

            // 'x' is used as the alias for the xmatch table
            if (SchemaManager.Comparer.Compare(Constants.XMatchDefaultAlias, alias) == 0)
            {
                c++;
                alias = a + c.ToString();
            }

            foreach (var catalog in catalogs)
            {
                if (SchemaManager.Comparer.Compare(catalog.Alias, alias) == 0)
                {
                    c++;
                    alias = a + c.ToString();
                }
            }

            return alias;
        }

        public void Validate(SchemaManager schemaManager)
        {
            if (catalogs.Count <= 1)
            {
                throw CreateException(ExceptionMessages.MoreCatalogsRequired);
            }

            if (columns.Count == 0)
            {
                throw CreateException(ExceptionMessages.XMatchColumnsRequired);
            }

            var alias = new HashSet<string>(SchemaManager.Comparer);
            alias.Add("x");

            foreach (var catalog in catalogs)
            {
                if (alias.Contains(catalog.Alias))
                {
                    throw CreateException(String.Format(ExceptionMessages.DuplicateAlias, catalog.Alias));
                }

                var ds = schemaManager.Datasets[catalog.DatasetName];
                var t = (TableOrView)ds.GetObject(catalog.TableReference.TableOrView.UniqueKey);

                if (t.PrimaryKey == null)
                {
                    throw CreateException(String.Format(ExceptionMessages.PrimaryKeyRequired, catalog.Alias));
                }

                // TODO: modify this if coordinates are looked up by schema
                if (catalog.CoordinateMode == CoordinateMode.Automatic)
                {
                    if (!t.Columns.ContainsKey(SkyQuery.Parser.TableCoordinates.RaColumnName))
                    {
                        throw CreateException(String.Format(ExceptionMessages.RaColumnNotFound, catalog.Alias));
                    }

                    if (!t.Columns.ContainsKey(SkyQuery.Parser.TableCoordinates.DecColumnName))
                    {
                        throw CreateException(String.Format(ExceptionMessages.DecColumnNotFound, catalog.Alias));
                    }
                }

                alias.Add(catalog.Alias);
            }
        }

        private SkyQueryCodeGeneratorException CreateException(string message)
        {
            return new SkyQueryCodeGeneratorException(message);
        }

        private SkyQueryCodeGeneratorException CreateException(string message, params object[] args)
        {
            var msg = String.Format(message, args);
            var ex = new SkyQueryCodeGeneratorException(msg);
            return ex;
        }
    }
}
