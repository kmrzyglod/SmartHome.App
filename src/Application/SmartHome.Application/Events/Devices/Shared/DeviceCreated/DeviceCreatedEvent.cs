using System;
using SmartHome.Application.Interfaces.Event;

namespace SmartHome.Application.Events.Devices.Shared.DeviceCreated
{
    public class DeviceCreatedEvent : IEvent
    {
        public DateTime Timestamp { get; set; }
        public string Source { get; set; } = string.Empty;
    }
}