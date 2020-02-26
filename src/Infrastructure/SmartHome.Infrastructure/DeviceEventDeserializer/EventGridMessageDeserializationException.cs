using System;

namespace SmartHome.Infrastructure.DeviceEventDeserializer
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