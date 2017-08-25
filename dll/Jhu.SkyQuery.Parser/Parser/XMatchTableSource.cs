using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;

namespace Jhu.SkyQuery.Parser
{

    public partial class XMatchTableSource : ITableReference, ITableSource
    {
        private TableReference tableReference;

        public override TableReference TableReference
        {
            get { return tableReference; }
            set { tableReference = value; }
        }

        public override ITableSource SpecificTableSource
        {
            get { return this; }
        }

        public bool IsSubquery
        {
            get { return false; }
        }

        public bool IsMultiTable
        {
            get { return true; }
        }

        public string XMatchAlgorithm
        {
            get { return FindDescendantRecursive<XMatchAlgorithm>().Value; }
        }

        public double XMatchLimit
        {
            get
            {
                return double.Parse(FindDescendant<XMatchConstraint>().FindDescendant<Number>().Value, System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        public string Alias
        {
            get { return FindDescendant<TableAlias>().Value; }
        }

        protected override void OnInitializeMembers()
        {
            base.OnInitializeMembers();

            this.tableReference = null;
        }

        protected override void OnCopyMembers(object other)
        {
            base.OnCopyMembers(other);

            var old = (XMatchTableSource)other;

            if (old != null)
            {
                this.tableReference = old.tableReference;
            }
        }
        
        public IEnumerable<XMatchTableSpecification> EnumerateXMatchTableSpecifications()
        {
            return this.FindDescendant<XMatchTableList>().EnumerateDescendants<XMatchTableSpecification>();
        }

        public IEnumerable<ITableSource> EnumerateSubqueryTableSources(bool recursive)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ITableSource> EnumerateMultiTableSources()
        {
            foreach (var xts in EnumerateDescendantsRecursive<XMatchTableSpecification>())
            {
                yield return xts.TableSource;
            }
        }
    }
}
