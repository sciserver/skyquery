using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;

namespace Jhu.SkyQuery.Parser
{
    public class BayesianXMatchTableSource : XMatchTableSource
    {
        private TableReference tableReference;

        public override TableReference TableReference
        {
            get { return tableReference; }
            set { tableReference = value; }
        }

        #region Constructors and initialiters

        public BayesianXMatchTableSource()
        {
            InitializeMembers();
        }

        public BayesianXMatchTableSource(XMatchTableSource old)
            :base(old)
        {
        }

        public BayesianXMatchTableSource(BayesianXMatchTableSource old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
        }

        private void CopyMembers(BayesianXMatchTableSource old)
        {
            this.tableReference = old.tableReference;
        }

        #endregion

        public override Node Interpret()
        {
            tableReference = new BayesianXMatchTableReference()
            {
                Alias = this.Alias
            };

            return base.Interpret();
        }


    }
}
