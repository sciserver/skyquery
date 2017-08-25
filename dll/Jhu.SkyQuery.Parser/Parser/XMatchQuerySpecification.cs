using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.SqlParser;

namespace Jhu.SkyQuery.Parser
{
    public partial class XMatchQuerySpecification
    {

        public XMatchTableSource XMatchTableSource
        {
            get { return this.FindDescendantRecursive<XMatchTableSource>(); }
        }
    }
}