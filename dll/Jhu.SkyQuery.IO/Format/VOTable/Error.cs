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

        public static VOTableException UnsupportedSerialization(VOTableSerialization serialization)
        {
            return new VOTableException(String.Format(ExceptionMessage.UnsupportedSerialization, serialization));
        }

        public static VOTableException ReferencedStreamsNotSupported()
        {
            return new VOTableException(ExceptionMessage.ReferencedStreamsNotSupported);
        }

        public static VOTableException EncodingNotFound()
        {
            return new VOTableException(ExceptionMessage.EncodingNotFound);
        }

        public static VOTableException EncodingNotSupported(string encoding)
        {
            return new VOTableException(String.Format(ExceptionMessage.EncodingNotSupported, encoding));
        }

        public static VOTableException MultidimensionalStringNotSupported()
        {
            return new VOTableException(ExceptionMessage.MultidimensionalStringNotSupported);
        }

        public static VOTableException PrimitiveArraysNotSupported()
        {
            return new VOTableException(ExceptionMessage.PrimitiveArraysNotSupported);
        }

        public static VOTableException BitNotSupported()
        {
            return new VOTableException(ExceptionMessage.BitNotSupported);
        }
    }
}
