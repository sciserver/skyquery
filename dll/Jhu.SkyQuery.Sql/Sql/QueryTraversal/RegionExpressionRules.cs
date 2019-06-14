using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Graywulf.Parsing;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.Graywulf.Sql.QueryTraversal;
using Jhu.SkyQuery.Sql.Parsing;

namespace Jhu.SkyQuery.Sql.QueryTraversal
{
    /// <summary>
    /// Implements a specialized version of the shuntling yard algorithm to output
    /// expressions in reverse polish notation.
    /// </summary>
    public class RegionExpressionRules : ExpressionReshufflerRules
    {
        public override void Route(Token node)
        {
            switch (node)
            {
                // TODO: right now only constants are allowed in the region shapes
                // but this could easily be updated to support variables and simple
                // expressions not referencing columns. Until then, just inline
                // RegionShape nodes. Later, shapes will need to be converted into
                // function-call-like structures and argument traversed.

                // Predicates are inlined and sent directly to the output
                case RegionShape n:
                    Inline(n);
                    break;

                // Default behavior: skip token
                default:
                    break;
            }
        }
    }
}
