using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;

namespace Jhu.SkyQuery.Parser
{
    public partial class CoordinateTableSource : ITableSource
    {
        // TODO: add coordinate lookup by metadata

        private const int positionIndex = 0;
        private const int errorIndex = 1;

        public TableOrViewName TableOrViewName
        {
            get { return FindDescendant<TableOrViewName>(); }
        }

        public TableReference TableReference
        {
            get { return TableOrViewName.TableReference; }
            set { TableOrViewName.TableReference = value; }
        }

        public bool IsSubquery
        {
            get { return false; }
        }

        public bool IsMultiTable
        {
            get { return false; }
        }

        private CoordinateHintList TableHintList
        {
            get { return FindDescendant<CoordinateHintClause>().FindDescendant<CoordinateHintList>(); }
        }

        public CoordinatePoint Position
        {
            get
            {               
                return TableHintList.FindDescendant<CoordinatePoint>();
            }
        }



        private ArgumentList ErrorArgumentList
        {
            get { return TableHintList.FindDescendantRecursive<CoordinateError>().FindDescendant<FunctionArguments>().FindDescendant<ArgumentList>(); }
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
            var node = (CoordinateTableSource)base.Interpret();

            // Look up table alias
            node.TableReference.InterpretTableSource((Node)this);

            return node;
        }

        public IEnumerable<ITableSource> EnumerateSubqueryTableSources(bool recursive)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ITableSource> EnumerateMultiTableSources()
        {
            throw new NotImplementedException();
        }
    }
}
