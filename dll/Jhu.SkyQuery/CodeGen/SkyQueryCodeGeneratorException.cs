using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.CodeGen
{
    [Serializable]
    public class SkyQueryCodeGeneratorException : Exception
    {
        public SkyQueryCodeGeneratorException()
        {
        }

        public SkyQueryCodeGeneratorException(string message)
            : base(message)
        {
        }

        public SkyQueryCodeGeneratorException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
