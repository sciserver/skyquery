using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.Jobs.Query
{
    [Serializable]
    public class XMatchException : Exception
    {
        public XMatchException()
        {
        }

        public XMatchException(string message)
            : base(message)
        {
        }
    }
}
