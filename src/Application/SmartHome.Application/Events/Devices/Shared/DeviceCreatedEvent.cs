using System;
using System.Collections.Generic;
using System.Text;
using SmartHome.Application.Interfaces.Event;

namespace SmartHome.Application.Events.Devices.Shared
{
    public class DeviceCreatedEvent: IEvent
    {
        public string Source { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
