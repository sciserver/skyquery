using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;

namespace Jhu.SkyQuery.Parser
{
    public partial class SelectStatement
    {
        public SelectStatement()
            : base()
        {
            InitializeMembers();
        }

        public SelectStatement(SelectStatement old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
        }

        private void CopyMembers(SelectStatement old)
        {
        }

        public override Node Interpret()
        {
            // Replace for specific type if neccesary
            if (FindDescendantRecursive<XMatchClause>() != null)
            {
                XMatchSelectStatement xms = new XMatchSelectStatement(this);
                xms.InterpretChildren();
                return xms;
            }
            else
            {
                return base.Interpret();
            }
        }
    }
}
