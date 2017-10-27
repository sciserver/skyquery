using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

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

        public static InvalidOperationException ConnectionNotOpen()
        {
            return new InvalidOperationException(ExceptionMessages.ConnectionNotOpen);
        }

        public static TapException UnexpectedHttpResponse(HttpStatusCode statusCode, string message)
        {
            return new TapException(String.Format(ExceptionMessages.UnexpectedHttpResponse, (int)statusCode, message));
        }

        public static TapException UnexpectedPhase(string phase)
        {
            return new Client.TapException(String.Format(ExceptionMessages.UnexpectedPhase, phase));
        }
    }
}
