using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
