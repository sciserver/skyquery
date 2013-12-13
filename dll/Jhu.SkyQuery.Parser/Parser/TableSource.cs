using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Parser
{
    public partial class TableSource
    {
        public override Graywulf.ParserLib.Node Interpret()
        {
            var xts = FindDescendant<XMatchTableSource>();
            if (xts != null)
            {
                SpecificTableSource = xts;
            }

            return base.Interpret();
        }
    }
}
