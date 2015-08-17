using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Parser
{
    public abstract class XMatchQuerySpecification : RegionQuerySpecification
    {

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
    }
}