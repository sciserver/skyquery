using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

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

        protected XMatchException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
