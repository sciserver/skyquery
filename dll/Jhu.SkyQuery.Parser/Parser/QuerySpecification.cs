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
        public override Node Interpret()
        {            
            // Look for specific nodes to decide on query type
            
            // --- XMatch query
            var from = FindDescendant<FromClause>();
            if (from != null)
            {
                var xts = from.FindDescendantRecursive<XMatchTableSource>();
                if (xts != null)
                {
                    base.Interpret();

                    XMatchQuerySpecification xmqs;
                    var algorithm = xts.XMatchAlgorithm;

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
            }

            // --- Region query (without an xmatch part)

            var region = FindDescendant<RegionClause>();
            if (region != null)
            {
                var rqs = new RegionQuerySpecification(this);
                rqs.InterpretChildren();
                return rqs;
            }

            return base.Interpret();
        }
    }
}
