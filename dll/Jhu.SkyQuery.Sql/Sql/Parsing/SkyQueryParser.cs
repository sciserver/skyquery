using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Jhu.Graywulf.Parsing;
using Jhu.Graywulf.Sql.Parsing;

namespace Jhu.SkyQuery.Sql.Parsing
{
    public partial class SkyQueryParser
    {
        protected override Node Exchange(Node node)
        {
            if (node is SimpleTableSource && node.FindAscendant<RegionSelectStatement>() != null)
            {
                return new CoordinatesTableSource((SimpleTableSource)node);
            }
            else if (node is XMatchTableSourceSpecification)
            {
                var xts = (XMatchTableSourceSpecification)node;
                var algorithm = xts.Algorithm;

                switch (algorithm.ToUpperInvariant())
                {
                    case Constants.AlgorithmBayesFactor:
                        return new BayesFactorXMatchTableSourceSpecification(xts);
                    case Constants.AlgorithmCone:
                        return new XMatchTableSourceSpecification(xts);
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