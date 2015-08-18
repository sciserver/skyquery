using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;

namespace Jhu.SkyQuery.Parser
{
    public class RegionSelectStatement : SelectStatement
    {
        #region Constructors and initializers

        public RegionClause RegionClause
        {
            get { return FindDescendantRecursive<RegionClause>(); }
        }

        public RegionSelectStatement()
            : base()
        {
        }

        public RegionSelectStatement(RegionSelectStatement old)
            : base(old)
        {
        }

        public RegionSelectStatement(SelectStatement old)
            : base(old)
        {
        }

        public RegionSelectStatement(Jhu.Graywulf.SqlParser.SelectStatement old)
            : base(old)
        {
        }

        #endregion
    }
}
