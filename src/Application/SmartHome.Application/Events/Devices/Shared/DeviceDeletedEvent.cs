using System;
using SmartHome.Application.Interfaces.Event;

namespace SmartHome.Application.Events.Devices.Shared
{
    public class DeviceDeletedEvent : IEvent
    {
        public DateTime Timestamp { get; set; }
        public string Source { get; set; } = string.Empty;
    }
}