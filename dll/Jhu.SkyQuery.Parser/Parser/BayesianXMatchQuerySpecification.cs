using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.SqlParser;

namespace Jhu.SkyQuery.Parser
{
    public class BayesianXMatchQuerySpecification : XMatchQuerySpecification
    {

        #region Constructors and initializers

        public BayesianXMatchQuerySpecification()
            : base()
        {
        }

        public BayesianXMatchQuerySpecification(QuerySpecification qs)
            :base(qs)
        {
        }

        public BayesianXMatchQuerySpecification(XMatchQuerySpecification xqs)
            :base(xqs)
        {
        }

        public BayesianXMatchQuerySpecification(BayesianXMatchQuerySpecification old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new BayesianXMatchQuerySpecification(this);
        }

        #endregion
    }
}
