using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;

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
            get { return (XMatchQuerySpecification)QueryExpression.FindDescendant<QuerySpecification>(); }
        }
        
        public XMatchSelectStatement(SelectStatement old)
            : base(old)
        {
        }
        
    }
}
