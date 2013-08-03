using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Parser
{
    public class BayesianXMatchTableSpecification : XMatchTableSpecification, ICloneable
    {
        public BayesianXMatchTableSpecification()
            : base()
        {
            InitializeMembers();
        }

        public BayesianXMatchTableSpecification(XMatchTableSpecification old)
            : base(old)
        {
            InitializeMembers();
        }

        public BayesianXMatchTableSpecification(BayesianXMatchTableSpecification old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
        }

        private void CopyMembers(BayesianXMatchTableSpecification old)
        {
        }

        public override object Clone()
        {
            return new BayesianXMatchTableSpecification(this);
        }
    }
}
