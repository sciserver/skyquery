using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;

namespace Jhu.SkyQuery.Parser
{
    public class XMatchSelectStatement : RegionSelectStatement
    {
        public override bool IsPartitioned
        {
            get { return true; }
        }

        public XMatchQuerySpecification XMatchQuerySpecification
        {
            get { return (XMatchQuerySpecification)QueryExpression.FindDescendant<QuerySpecification>(); }
        }

        public XMatchSelectStatement()
            : base()
        {
        }

        public XMatchSelectStatement(XMatchSelectStatement old)
            : base(old)
        {
        }

        public XMatchSelectStatement(SelectStatement old)
            : base(old)
        {
        }

        public override object Clone()
        {
            return new XMatchSelectStatement(this);
        }
    }
}
