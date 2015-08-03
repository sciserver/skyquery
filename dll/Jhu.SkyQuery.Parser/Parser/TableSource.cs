using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Parser
{
    public partial class TableSource
    {
        protected override Graywulf.SqlParser.ITableSource FindSpecificTableSource()
        {
            var cts = FindDescendant<CoordinateTableSource>();
            if (cts != null)
            {
                return cts;
            }

            var xts = FindDescendant<XMatchTableSource>();
            if (xts != null)
            {
                return xts;
            }

            return base.FindSpecificTableSource();
        }
    }
}
