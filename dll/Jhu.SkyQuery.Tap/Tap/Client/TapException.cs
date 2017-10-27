using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.Tap.Client
{
    public class TapException : System.Data.Common.DbException
    {
        public TapException()
        {
        }

        public TapException(string message)
            : base(message)
        {
        }

        public TapException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
