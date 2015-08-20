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
        #region Constructors and initialiters

        public BayesianXMatchTableSource()
        {
        }

        public BayesianXMatchTableSource(XMatchTableSource old)
            :base(old)
        {
        }

        public BayesianXMatchTableSource(BayesianXMatchTableSource old)
            : base(old)
        {
        }

        public override object Clone()
        {
            return new BayesianXMatchTableSource(this);
        }

        #endregion

        public override void Interpret()
        {
 	        base.Interpret();

            TableReference = new BayesianXMatchTableReference()
            {
                Alias = this.Alias
            };
        }
    }
}
