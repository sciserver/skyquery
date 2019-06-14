using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Sql.Parsing
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

        protected override void OnInterpret()
        {
            TableReference = new NameResolution.BayesFactorXMatchTableReference()
            {
                Alias = this.Alias
            };
        }
    }
}
