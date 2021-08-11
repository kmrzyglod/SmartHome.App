using System;

namespace SmartHome.Infrastructure.EventBus
{
    public class EventBusException : Exception
    {
        public EventBusException()
        {
        }

        public EventBusException(string message) : base(message)
        {
        }

        public EventBusException(string message, Exception innerException) : base(message,
            innerException)
        {
        }
    }
}