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
        public virtual TableReference TableReference
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
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

        #region Constructors and initialiters

        public XMatchTableSource()
        {
            InitializeMembers();
        }

        public XMatchTableSource(XMatchTableSource old)
            :base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
        }

        private void CopyMembers(XMatchTableSource old)
        {
        }

        #endregion

        public override Node Interpret()
        {
            switch (XMatchAlgorithm.ToUpper())
            {
                case Constants.AlgorithmBayesFactor:
                    var xts = new BayesianXMatchTableSource(this);
                    xts.InterpretChildren();
                    return xts;
                default:
                    throw new NotImplementedException();
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
