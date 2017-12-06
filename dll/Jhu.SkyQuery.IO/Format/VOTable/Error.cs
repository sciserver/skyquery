using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.Format.VOTable
{
    public static class Error
    {
        public static VOTableException RecursiveResourceNotSupported()
        {
            return new VOTableException(ExceptionMessage.RecursiveResourceNotSupported);
        }
        public static VOTableException TypeofNamespaceNotSupported()
        {
            return new VOTableException(ExceptionMessage.TypeofNamespaceNotSupported);
        }
    }
}
