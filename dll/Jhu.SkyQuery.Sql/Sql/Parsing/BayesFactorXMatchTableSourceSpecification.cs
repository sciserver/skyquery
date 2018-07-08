using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Sql.Parsing
{
    public class BayesFactorXMatchTableSourceSpecification : XMatchTableSourceSpecification
    {
        #region Constructors and initialiters

        public BayesFactorXMatchTableSourceSpecification()
        {
        }

        public BayesFactorXMatchTableSourceSpecification(XMatchTableSourceSpecification old)
            :base(old)
        {
        }

        public BayesFactorXMatchTableSourceSpecification(BayesFactorXMatchTableSourceSpecification old)
            : base(old)
        {
        }

        public override object Clone()
        {
            return new BayesFactorXMatchTableSourceSpecification(this);
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
