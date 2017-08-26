using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Parsing;

namespace Jhu.SkyQuery.Parser
{
    public partial class XMatchSelectStatement
    {
        public override bool IsPartitioned
        {
            get { return true; }
        }

        public XMatchQuerySpecification XMatchQuerySpecification
        {
            get { return QueryExpression.FindDescendant<XMatchQuerySpecification>(); }
        }

    }
}
