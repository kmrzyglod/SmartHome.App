using System;

namespace SmartHome.Infrastructure.CommandBusMessageDeserializer
{
    public class ServiceBusMessageDeserializationException : Exception
    {
        public ServiceBusMessageDeserializationException()
        {
        }

        public ServiceBusMessageDeserializationException(string message) : base(message)
        {
        }

        public ServiceBusMessageDeserializationException(string message, Exception innerException) : base(message,
            innerException)
        {
        }
    }
}