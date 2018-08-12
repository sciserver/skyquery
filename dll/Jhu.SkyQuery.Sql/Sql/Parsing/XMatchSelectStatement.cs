using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Parsing;

namespace Jhu.SkyQuery.Sql.Parsing
{
    public partial class XMatchSelectStatement
    {
#if false
        public override bool IsPartitioned
        {
            get { return true; }
        }

#endif
        public XMatchQuerySpecification XMatchQuerySpecification
        {
            get { return QueryExpression.FindDescendant<XMatchQuerySpecification>(); }
        }

    }
}
