using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;

namespace Jhu.SkyQuery.Parser
{
    public partial class XMatchTableSource : ITableSource
    {
        public TableOrViewName TableOrViewName
        {
            get { return FindDescendant<TableOrViewName>(); }
        }

        public TableReference TableReference
        {
            get { return TableOrViewName.TableReference; }
            set { TableOrViewName.TableReference = value; }
        }

        private XMatchTableHintList TableHintList
        {
            get { return FindDescendant<XMatchHintClause>().FindDescendant<XMatchTableHintList>(); }
        }

        public XMatchPoint Position
        {
            get
            {
                return TableHintList.FindDescendant<XMatchPoint>();
            }
        }

        private ArgumentList ErrorArgumentList
        {
            get { return TableHintList.FindDescendantRecursive<XMatchError>().FindDescendant<FunctionArguments>().FindDescendant<ArgumentList>(); }
        }

        public Expression ErrorExpression
        {
            get
            {
                var ar = ErrorArgumentList.FindDescendant<Argument>(0);
                return ar == null ? null : ar.FindDescendant<Expression>();
            }
        }

        public bool IsConstantError
        {
            get
            {
                var ars = ErrorArgumentList.EnumerateDescendants<Argument>().ToArray();
                return ars.Length == 1 && ars[0].FindDescendant<Expression>().IsConstantNumber;
            }
        }

        public Expression MinErrorExpression
        {
            get
            {
                var ar = ErrorArgumentList.FindDescendant<Argument>(1);
                return ar == null ? null : ar.FindDescendant<Expression>();
            }
        }

        public Expression MaxErrorExpression
        {
            get
            {
                var ar = ErrorArgumentList.FindDescendant<Argument>(2);
                return ar == null ? null : ar.FindDescendant<Expression>();
            }
        }

        public override Node Interpret()
        {
            var node = (XMatchTableSource)base.Interpret();

            // Look up table alias
            node.TableReference.InterpretTableSource((Node)this);

            return node;
        }
    }
}
