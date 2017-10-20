using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.Tap.Client
{
    public class Error
    {
        public static ArgumentException InvalidTapCommandType()
        {
            return new ArgumentException(ExceptionMessages.InvalidTapCommandType);
        }

        public static KeyNotFoundException ParameterNotFound(string key)
        {
            return new KeyNotFoundException(String.Format(ExceptionMessages.ParameterNotFound, key));
        }
    }
}
