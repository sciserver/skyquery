using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Parser
{
    public class BayesianXMatchClause : XMatchClause, ICloneable
    {
        public BayesianXMatchClause()
            : base()
        {
            InitializeMembers();
        }

        public BayesianXMatchClause(XMatchClause old)
            : base(old)
        {
            InitializeMembers();
        }

        public BayesianXMatchClause(BayesianXMatchClause old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
        }

        private void CopyMembers(BayesianXMatchClause old)
        {
        }

        public override object Clone()
        {
            return new BayesianXMatchClause(this);
        }

    }
}
