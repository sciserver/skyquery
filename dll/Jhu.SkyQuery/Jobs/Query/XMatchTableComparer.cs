using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.SkyQuery.Parser;

namespace Jhu.SkyQuery.Jobs.Query
{
    public class XMatchTableComparer : IComparer<XMatchTableSpecification>
    {
        private QueryObject queryObject;

        public XMatchTableComparer(QueryObject queryObject)
        {
            this.queryObject = queryObject;
        }

        public int Compare(XMatchTableSpecification x, XMatchTableSpecification y)
        {
            // Inclusion method ordering always preceeds table statistics
            if (x.InclusionMethod != y.InclusionMethod)
            {
                return y.InclusionMethod - x.InclusionMethod;
            }

            // Order tables by cardinality
            TableStatistics xts = null;
            TableStatistics yts = null;

            if (queryObject.TableStatistics.ContainsKey(x.TableSource.UniqueKey))
            {
                xts = queryObject.TableStatistics[x.TableSource.UniqueKey];
            }

            if (queryObject.TableStatistics.ContainsKey(y.TableSource.UniqueKey))
            {
                yts = queryObject.TableStatistics[y.TableSource.UniqueKey];
            }

            if (xts == null && yts == null)
            {
                return 0;
            }
            else if (xts == null)
            {
                return -1;
            }
            else if (yts == null)
            {
                return 1;
            }
            else
            {
                return Math.Sign(xts.RowCount - yts.RowCount);
            }
        }
    }
}
