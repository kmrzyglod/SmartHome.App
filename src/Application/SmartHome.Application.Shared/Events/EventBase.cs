using System;
using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Application.Shared.Events
{
    public abstract class EventBase: IEvent
    {
        public string Source { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
