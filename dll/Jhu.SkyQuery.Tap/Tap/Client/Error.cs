﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public static TapException UnexpectedHttpResponse(HttpStatusCode statusCode, string message, string body)
        {
            return new TapException(String.Format(ExceptionMessages.UnexpectedHttpResponse, (int)statusCode, message, body));
        }

        public static TapException ServiceError(HttpStatusCode statusCode, string message, string body)
        {
            return new TapException(String.Format(ExceptionMessages.ServiceError, (int)statusCode, message, body));
        }

        public static TapException UnexpectedPhase(string phase)
        {
            return new TapException(String.Format(ExceptionMessages.UnexpectedPhase, phase));
        }

        public static TapException UnsupportedQueryLanguage(TapQueryLanguage language)
        {
            return new TapException(String.Format(ExceptionMessages.UnsupportedQueryLanguage, language.ToString()));
        }

        public static InvalidOperationException CommandExecuting()
        {
            return new InvalidOperationException(ExceptionMessages.CommandExecuting);
        }

        public static InvalidOperationException CommandNotExecuting()
        {
            return new InvalidOperationException(ExceptionMessages.CommandNotExecuting);
        }

        public static TapException CommunicationException(Exception innerException)
        {
            return new TapException(ExceptionMessages.CommunicationException, innerException);
        }

        public static TapException CommandTimeout()
        {
            return new TapException(ExceptionMessages.CommandTimeout);
        }

        public static TapException CommandCancelled()
        {
            return new TapException(ExceptionMessages.CommandCancelled);
        }

        public static TapException CommandCancelled(Exception innerException)
        {
            return new TapException(ExceptionMessages.CommandCancelled, innerException);
        }

        public static OperationCanceledException OperationCancelled(CancellationToken token)
        {
            return new OperationCanceledException(token);
        }

        public static TapException ServiceNotAvailable(VO.Vosi.Availability.Common.IAvailability avail)
        {
            var ex = new TapException(String.Format(ExceptionMessages.ServiceNotAvailable, avail.Note));
            return ex;
        }
        
        public static TapException DeserializationException(System.Xml.XmlException ex)
        {
            return new TapException(ExceptionMessages.DeserializationException, ex);
        }

        public static TapException TooManyRedirects()
        {
            return new TapException(ExceptionMessages.TooManyRedirects);
        }
    }
}
