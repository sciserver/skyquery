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

        protected override void DispatchQuerySpecification(Graywulf.Sql.Parsing.QuerySpecification node)
        {
            switch (node)
            {
                case XMatchQuerySpecification n:
                    TraverseRegionQuerySpecification(n);
                    break;
                //case RegionQuerySpecification n:
                //    TraverseRegionQuerySpecification(n);
                //    break;
                default:
                    base.DispatchQuerySpecification(node);
                    break;
            }
        }

        protected override void DispatchTableSource(TableSource node)
        {

            // Xmatchtable source that inherits from table source

            // but need to traverse xmatchspecification separately

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

        #region Region clause

        protected void TraverseRegionQuerySpecification(RegionQuerySpecification node)
        {
            var region = node.FindDescendant<RegionClause>();

            if (region != null)
            {
                TraverseRegionClause(region);
            }

            base.TraverseQuerySpecification(node);
        }

        protected void TraverseRegionClause(RegionClause node)
        {
            foreach (var nn in node.Stack)
            {
                switch (nn)
                {
                    case Literal n:
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

        protected void TraverseRegionExpression(RegionExpression node)
        {
            // TODO: create reshuffler and select context if necessary

            //CreateLogicalExpressionReshuffler();
            TraverseRegionExpressionNode(node);
            //DestroyExpressionReshuffler();
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
            if (QueryContext.HasFlag(QueryTraversal.QueryContext.RegionExpression))
            {
                VisitInlineNode(node);
            }
            else
            {
                foreach (var nn in ((Node)node.Stack.First).Stack)
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

                VisitNode(node);
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
        #region XMatch

        private void TraverseXMatchTableSource(XMatchTableSource node)
        {
        }

        #endregion
    }
}
