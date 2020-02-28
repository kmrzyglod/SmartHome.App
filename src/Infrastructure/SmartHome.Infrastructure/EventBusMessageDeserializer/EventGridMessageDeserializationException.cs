using System;

namespace SmartHome.Infrastructure.EventBusMessageDeserializer
{
    public class EventGridMessageDeserializationException : Exception
    {
        public EventGridMessageDeserializationException()
        {
        }

        public EventGridMessageDeserializationException(string message) : base(message)
        {
        }

        public EventGridMessageDeserializationException(string message, Exception innerException) : base(message,
            innerException)
        {
        }
    }
}