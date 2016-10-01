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
        private string targetTable;
        private double bayesFactor;
        private string region;
        private List<string> columns;
        private List<Catalog> catalogs;

        public string TargetTable
        {
            get { return targetTable; }
            set { targetTable = value; }
        }

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
            this.targetTable = "xmatchtable";
            this.bayesFactor = 1e3;
            this.region = null;
            this.columns = new List<string>();
            this.catalogs = new List<Catalog>();
        }

        public Catalog AddCatalog(TableOrView table)
        {
            var catalog = new Catalog()
            {
                Alias = GenerateAlias(table),
                DatasetName = table.Dataset.Name,
                TableUniqueKey = table.UniqueKey
            };


            catalogs.Add(catalog);

            return catalog;
        }

        private string GenerateAlias(TableOrView table)
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

        public void Validate()
        {
            // TODO
        }
    }
}
