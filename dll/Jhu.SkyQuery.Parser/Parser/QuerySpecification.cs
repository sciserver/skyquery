using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;

namespace Jhu.SkyQuery.Parser
{
    public partial class QuerySpecification
    {
        public QuerySpecification()
            : base()
        {
            InitializeMembers();
        }

        public QuerySpecification(QuerySpecification old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
        }

        private void CopyMembers(QuerySpecification old)
        {
        }

        public override Node Interpret()
        {            
            // Replace for specific type if neccesary
            XMatchClause xmc = FindDescendant<XMatchClause>();
            if (xmc != null)
            {
                base.Interpret();

                string algorithm = xmc.XMatchAlgorithm;
                XMatchQuerySpecification xmqs;
                if (SkyQueryParser.ComparerInstance.Compare(algorithm, Constants.AlgorithmBayesFactor) == 0)
                {
                    xmqs = new BayesianXMatchQuerySpecification(this);
                }
                else
                {
                    throw new NotImplementedException();
                }
                xmqs.InterpretChildren();
                return xmqs;
            }
            else
            {
                return base.Interpret();
            }
        }
    }
}
