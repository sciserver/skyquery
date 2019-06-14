using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.Sql.QueryRewriting
{
    public class SkyQuerySqlQueryRewriter : Jhu.Graywulf.Sql.Extensions.QueryRewriting.GraywulfSqlQueryRewriter
    {
#if false

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

#endif
    }
}
