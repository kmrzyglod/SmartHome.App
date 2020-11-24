using System;

namespace SmartHome.Application.Exceptions
{
    public class HttpResponseMessageDeserializationException : Exception
    {
        public HttpResponseMessageDeserializationException(string message)
            : base(message)
        {
        }
    }
}