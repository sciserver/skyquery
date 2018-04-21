using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Jhu.Graywulf.Parsing;
using Jhu.Graywulf.Sql.Parsing;

namespace Jhu.SkyQuery.Parser
{
    public partial class SkyQueryParser
    {
        protected override Node Exchange(Node node)
        {
            if (node is SimpleTableSource && node.FindAscendant<RegionSelectStatement>() != null)
            {
                return new CoordinatesTableSource((SimpleTableSource)node);
            }
            else if (node is XMatchTableSource)
            {
                var xts = (XMatchTableSource)node;
                var algorithm = xts.Algorithm;

                switch (algorithm.ToUpperInvariant())
                {
                    case Constants.AlgorithmBayesFactor:
                        return new BayesFactorXMatchTableSource(xts);
                    case Constants.AlgorithmCone:
                        return new ConeXMatchTableSource(xts);
                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                return base.Exchange(node);
            }
        }
    }
}