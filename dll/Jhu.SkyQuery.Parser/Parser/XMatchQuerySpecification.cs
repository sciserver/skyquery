using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Parser
{
    public partial class XMatchQuerySpecification : RegionQuerySpecification
    {

        public XMatchTableSource XMatchTableSource
        {
            get { return this.FindDescendantRecursive<XMatchTableSource>(); }
        }
        
        public XMatchQuerySpecification(QuerySpecification old)
            :base(old)
        {
        }
    }
}