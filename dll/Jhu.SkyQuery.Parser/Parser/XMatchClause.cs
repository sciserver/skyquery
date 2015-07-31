using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;

namespace Jhu.SkyQuery.Parser
{
    public partial class XMatchClause : ICloneable
    {
        public string Alias
        {
            get { return FindDescendant<TableAlias>().Value; }
        }

        public string XMatchAlgorithm
        {
            get { return FindDescendant<XMatchAlgorithm>().Value; }
        }

        #region Constructors and initializers

        internal XMatchClause()
        {
            InitializeMembers();
        }

        internal XMatchClause(XMatchClause old)
            :base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
        }

        private void CopyMembers(XMatchClause old)
        {
        }

        public virtual object Clone()
        {
            return new XMatchClause(this);
        }

        #endregion

        public override Node Interpret()
        {
            // Interpret XMatch Algorithm
            XMatchAlgorithm xma = FindDescendant<XMatchAlgorithm>();

            switch (xma.Value.ToUpper())
            {
                case Constants.AlgorithmBayesFactor:
                    BayesianXMatchClause xmc = new BayesianXMatchClause(this);
                    xmc.InterpretChildren();
                    return xmc;
                default:
                    return base.Interpret();
            }
        }

        public IEnumerable<XMatchTableSpecification> EnumerateXMatchTableSpecifications()
        {
            return this.FindDescendant<XMatchTableList>().EnumerateDescendants<XMatchTableSpecification>();
        }
    }
}
