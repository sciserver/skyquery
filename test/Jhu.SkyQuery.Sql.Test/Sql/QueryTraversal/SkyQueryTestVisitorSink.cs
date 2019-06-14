using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Jhu.SkyQuery.Sql.Parsing;
using Jhu.Graywulf.Sql.QueryTraversal;

namespace Jhu.SkyQuery.Sql.QueryTraversal
{
    public class SkyQueryTestVisitorSink : Jhu.Graywulf.Sql.QueryTraversal.TestVisitorSink
    {
        public SkyQueryTestVisitorSink()
        {
            visitor = new SkyQuerySqlQueryVisitor(this)
            {
                Options = new SkyQuerySqlQueryVisitorOptions()
                {
                    ExpressionTraversal = ExpressionTraversalMethod.Infix,
                    LogicalExpressionTraversal = ExpressionTraversalMethod.Infix,
                    VisitExpressionSubqueries = false,
                    VisitPredicateSubqueries = false,
                    VisitLiterals = true,
                    VisitSymbols = true,
                }
            };
        }

        public string Execute(RegionExpression node, ExpressionTraversalMethod direction)
        {
            ((SkyQuerySqlQueryVisitor)visitor).Options.RegionExpressionTraversal = direction;

            using (w = new StringWriter())
            {
                ((SkyQuerySqlQueryVisitor)visitor).Execute(node);
                return w.ToString();
            }
        }
    }
}
