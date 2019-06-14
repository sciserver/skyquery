using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Graywulf.Parsing;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.Graywulf.Sql.QueryTraversal;
using Jhu.Graywulf.Sql.Extensions.QueryTraversal;
using Jhu.SkyQuery.Sql.Parsing;

namespace Jhu.SkyQuery.Sql.QueryTraversal
{
    public class SkyQuerySqlQueryVisitor : GraywulfSqlQueryVisitor
    {
        #region Properties

        public new SkyQuerySqlQueryVisitorOptions Options
        {
            get { return (SkyQuerySqlQueryVisitorOptions)base.Options; }
            set { base.Options = value; }
        }

        #endregion
        #region Constructors and initializers

        public SkyQuerySqlQueryVisitor(SqlQueryVisitorSink sink)
            : base(sink)
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
        }

        protected override SqlQueryVisitorOptions CreateOptions()
        {
            return new GraywulfSqlQueryVisitorOptions();
        }

        #endregion

        public void Execute(RegionExpression node)
        {
            PushAllContextNone();
            TraverseRegionExpression(node);
            PopAllContext();
        }

        protected override void DispatchInlineNode(Token node)
        {
            switch (node)
            {
                case RegionShape n:
                    TraverseRegionShape(n);
                    break;
                default:
                    base.DispatchInlineNode(node);
                    break;
            }
        }
        
        protected override void DispatchQuerySpecification(Graywulf.Sql.Parsing.QuerySpecification node)
        {
            switch (node)
            {
                case RegionQuerySpecification n:
                    TraverseRegionQuerySpecification(n);
                    break;
                default:
                    base.DispatchQuerySpecification(node);
                    break;
            }
        }

        protected override void DispatchTableSource(TableSource node)
        {
            switch (node)
            {
                case XMatchTableSource n:
                    TraverseXMatchTableSource(n);
                    break;
                default:
                    base.DispatchTableSource(node);
                    break;
            }
        }

        #region XMatch

        private void TraverseXMatchTableSource(XMatchTableSource node)
        {
            VisitNode(node, 0);

            foreach (var nn in node.Stack)
            {
                switch (nn)
                {
                    case Literal n:
                        VisitNode(n);
                        break;
                    case BracketOpen n:
                        VisitNode(n);
                        break;
                    case BracketClose n:
                        VisitNode(n);
                        break;
                    case Comma n:
                        VisitNode(n);
                        break;
                    case TableAlias n:
                        VisitNode(n);
                        break;
                    case XMatchTableList n:
                        TraverseXMatchTableList(n);
                        break;
                    case XMatchConstraint n:
                        TraverseXMatchConstraint(n);
                        break;
                }
            }

            VisitNode(node, 1);
        }

        private void TraverseXMatchTableList(XMatchTableList node)
        {
            foreach (var nn in node.Stack)
            {
                switch (nn)
                {
                    case Comma n:
                        VisitNode(n);
                        break;
                    case XMatchTableSpecification n:
                        TraverseXMatchTableSpecification(n);
                        break;
                    case XMatchTableList n:
                        TraverseXMatchTableList(n);
                        break;
                }
            }
        }

        private void TraverseXMatchTableSpecification(XMatchTableSpecification node)
        {
            foreach (var nn in node.Stack)
            {
                switch (nn)
                {
                    case XMatchTableInclusion n:
                        TraverseXMatchTableInclusion(n);
                        break;
                    case TableSource n:
                        DispatchTableSource(n);
                        break;
                }
            }

            VisitNode(node);
        }

        private void TraverseXMatchTableInclusion(XMatchTableInclusion node)
        {
            foreach (var nn in node.Stack)
            {
                switch (nn)
                {
                    case Literal n:
                        VisitNode(n);
                        break;
                }
            }

            VisitNode(node);
        }

        private void TraverseXMatchConstraint(XMatchConstraint node)
        {
            foreach (var nn in node.Stack)
            {
                switch (nn)
                {
                    case Literal n:
                        VisitNode(n);
                        break;
                    case XMatchAlgorithm n:
                        TraverseXMatchAlgorithm(n);
                        break;
                    case NumericConstant n:
                        VisitNode(n);
                        break;
                    case Sql.Parsing.TableHint n:
                        TraverseTableHint(n);
                        break;
                }
            }

            VisitNode(node);
        }

        private void TraverseXMatchAlgorithm(XMatchAlgorithm node)
        {
            foreach (var nn in node.Stack)
            {
                switch (nn)
                {
                    case Literal n:
                        VisitNode(n);
                        break;
                }
            }
        }

        protected override void TraverseSimpleTableSource(Graywulf.Sql.Parsing.SimpleTableSource node)
        {
            // Figure out if it's an XMatched table and do the necessary stuff

            base.TraverseSimpleTableSource(node);
        }

        #endregion
        #region Region clause

        protected void TraverseRegionQuerySpecification(RegionQuerySpecification node)
        {
            base.TraverseQuerySpecification(node);

            var region = node.FindDescendant<RegionClause>();

            if (region != null)
            {
                TraverseRegionClause(region);
            }
        }

        protected void TraverseRegionClause(RegionClause node)
        {
            foreach (var nn in node.Stack)
            {
                switch (nn)
                {
                    case Literal n:
                        VisitNode(n);
                        break;
                    case StringConstant n:
                        VisitNode(n);
                        break;
                    case RegionExpression n:
                        TraverseRegionExpression(n);
                        break;
                }
            }
        }
                
        private void TraverseRegionExpression(RegionExpression node)
        {
            PushQueryContext(QueryContext | QueryContextExtensions.RegionExpression);

            PushExpressionReshuffler(Options.RegionExpressionTraversal, new RegionExpressionRules());
            TraverseRegionExpressionNode(node);
            PopExpressionReshuffler();

            PopQueryContext();
        }

        private void TraverseRegionExpressionNode(RegionExpression node)
        {
            foreach (var nn in SelectDirection(node.Stack))
            {
                switch (nn)
                {
                    case RegionNotOperator n:
                        VisitNode(n);
                        break;
                    case RegionOperator n:
                        VisitNode(n);
                        break;
                    case RegionExpressionBrackets n:
                        TraverseRegionExpressionBrackets(n);
                        break;
                    case RegionShape n:
                        TraverseRegionShape(n);
                        break;
                    case RegionExpression n:
                        TraverseRegionExpressionNode(n);
                        break;
                }
            }
        }

        private void TraverseRegionExpressionBrackets(RegionExpressionBrackets node)
        {
            foreach (var nn in SelectDirection(node.Stack))
            {
                switch (nn)
                {
                    case BracketOpen n:
                        VisitNode(n);
                        break;
                    case RegionExpression n:
                        TraverseRegionExpressionNode(n);
                        break;
                    case BracketClose n:
                        VisitNode(n);
                        break;
                }
            }
        }

        private void TraverseRegionShape(RegionShape node)
        {
            if (QueryContext.HasFlag(QueryContextExtensions.RegionExpression))
            {
                VisitInlineNode(node);
            }
            else
            {
                foreach (var nn in node.Stack)
                {
                    switch (nn)
                    {
                        case Literal n:
                            VisitNode(n);
                            break;
                        case BracketOpen n:
                            VisitNode(n);
                            break;
                        case BracketClose n:
                            VisitNode(n);
                            break;
                        case RegionArgumentList n:
                            TraverseRegionArgumentList(n);
                            break;
                    }
                }
            }
        }

        private void TraverseRegionArgumentList(RegionArgumentList node)
        {
            foreach (var nn in node.Stack)
            {
                switch (nn)
                {
                    case RegionArgument n:
                        TraverseRegionArgument(n);
                        break;
                    case Comma n:
                        VisitNode(n);
                        break;
                    case RegionArgumentList n:
                        TraverseRegionArgumentList(n);
                        break;
                }
            }
        }

        private void TraverseRegionArgument(RegionArgument node)
        {
            VisitNode((NumericConstant)node.Stack.First);
            VisitNode(node);
        }

        #endregion
    }
}
