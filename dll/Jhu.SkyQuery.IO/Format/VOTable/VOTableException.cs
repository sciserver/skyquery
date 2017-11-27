using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.Format.VOTable
{
    [Serializable]
    public class VOTableException : Exception
    {
        public VOTableException()
        {
        }

        public VOTableException(string message)
            : base(message)
        {
        }

        public VOTableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
