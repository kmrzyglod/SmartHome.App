using System;
using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Application.Events.Devices.Shared.DeviceDeleted
{
    public class DeviceDeletedEvent : IEvent
    {
        public DateTime Timestamp { get; set; }
        public string Source { get; set; } = string.Empty;
    }
}