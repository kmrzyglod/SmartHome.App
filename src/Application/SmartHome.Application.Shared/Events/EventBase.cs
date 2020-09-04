using System;
using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Application.Shared.Events
{
    public abstract class EventBase: IEvent
    {
        public string Source { get; set; } = null!;
        public DateTime Timestamp { get; set; }
    }
}
