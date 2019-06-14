using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.Sql.Parsing
{
    public partial class XMatchConstraint
    {
        public string Algorithm
        {
            get { return FindDescendant<XMatchAlgorithm>().Value.ToUpperInvariant(); }
        }
    }
}
