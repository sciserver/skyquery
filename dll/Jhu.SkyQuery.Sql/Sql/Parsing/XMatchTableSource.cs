using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.Graywulf.Sql.NameResolution;

namespace Jhu.SkyQuery.Sql.Parsing
{

    public partial class XMatchTableSource : ITableReference
    {
        private TableReference tableReference;
        private string uniqueKey;
        
        public override TableReference TableReference
        {
            get { return tableReference; }
            set { tableReference = value; }
        }

        public override string UniqueKey
        {
            get { return uniqueKey; }
            set { uniqueKey = value; }
        }
        
        public override bool IsSubquery
        {
            get { return false; }
        }

        public override bool IsMultiTable
        {
            get { return true; }
        }
        
        public string Alias
        {
            get { return FindDescendant<TableAlias>().Value; }
        }

        public XMatchConstraint Constraint
        {
            get { return FindDescendant<XMatchConstraint>(); }
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

        protected override void OnInterpret()
        {
            base.OnInterpret();

            switch (Constraint.Algorithm)
            {
                case Constants.AlgorithmBayesFactor:
                    ReplaceWith(new BayesFactorXMatchTableSource(this)).Interpret();
                    break;
                case Constants.AlgorithmCone:
                    ReplaceWith(new ConeXMatchTableSource(this)).Interpret();
                    break;
                case Constants.AlgorithmChi2:
                    throw new NotImplementedException();
            }
        }
    }
}
