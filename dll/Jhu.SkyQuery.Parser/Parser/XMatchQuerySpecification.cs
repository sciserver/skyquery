using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Parser
{
    public abstract class XMatchQuerySpecification : QuerySpecification
    {

        // TODO: delete, not used
        /*
        public SearchCondition XMatchConditions
        {
            get
            {
                return this.FindDescendant<XMatchClause>().FindDescendant<XMatchHavingClause>().FindDescendant<SearchCondition>();
            }
        }*/

        public XMatchTableSource XMatchTableSource
        {
            get { return this.FindDescendantRecursive<XMatchTableSource>(); }
        }

        public XMatchQuerySpecification()
            : base()
        {
        }

        public XMatchQuerySpecification(XMatchQuerySpecification old)
            :base(old)
        {
        }

        public XMatchQuerySpecification(QuerySpecification old)
            :base(old)
        {
        }

        // TODO: delete if not used
        /*
        public IEnumerable<XMatchTableSpecification> EnumerateXMatchTableSpecifications()
        {
            return this.FindDescendant<XMatchClause>().FindDescendant<XMatchTableList>().EnumerateDescendants<XMatchTableSpecification>();
        }*/
    }
}