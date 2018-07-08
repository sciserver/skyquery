using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.Graywulf.Sql.NameResolution;

namespace Jhu.SkyQuery.Sql.Parsing
{

    public partial class XMatchTableSourceSpecification : ITableReference, ITableSource
    {
        private TableReference tableReference;
        private string uniqueKey;
        
        public override TableReference TableReference
        {
            get { return tableReference; }
            set { tableReference = value; }
        }

        public string UniqueKey
        {
            get { return uniqueKey; }
            set { uniqueKey = value; }
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

        public string Algorithm
        {
            get { return FindDescendantRecursive<XMatchAlgorithm>().Value; }
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

            var old = (XMatchTableSourceSpecification)other;

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
