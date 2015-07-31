﻿using System;
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
            // Look for descentant nodes in the parsing tree to determine
            // query type. An XMatchClause means it's a cross-match query.
            // If only a RegionClause is present, it's a simpler region query.

            if (FindDescendantRecursive<XMatchClause>() != null)
            {
                var xms = new XMatchSelectStatement(this);
                xms.InterpretChildren();
                return xms;
            }
            else if (FindDescendantRecursive<RegionClause>() != null)
            {
                var rs = new RegionSelectStatement(this);
                rs.InterpretChildren();
                return rs;
            }
            else
            {
                return base.Interpret();
            }
        }
    }
}
