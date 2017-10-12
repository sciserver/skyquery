using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Parser
{
    public class BayesFactorXMatchTableSource : XMatchTableSource
    {
        #region Constructors and initialiters

        public BayesFactorXMatchTableSource()
        {
        }

        public BayesFactorXMatchTableSource(XMatchTableSource old)
            :base(old)
        {
        }

        public BayesFactorXMatchTableSource(BayesFactorXMatchTableSource old)
            : base(old)
        {
        }

        public override object Clone()
        {
            return new BayesFactorXMatchTableSource(this);
        }

        #endregion

        public override void Interpret()
        {
 	        base.Interpret();

            TableReference = new BayesFactorXMatchTableReference()
            {
                Alias = this.Alias
            };
        }
    }
}
