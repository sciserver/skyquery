using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;

namespace Jhu.SkyQuery.Parser
{
    public class XMatchSelectStatement : SelectStatement
    {
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
    }
}
