﻿using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Application.Shared.Events.Devices.Shared.DeviceStatusUpdated
{
    public class DeviceStatusUpdatedEvent : EventBase
    {
        public string Message { get; set; } = string.Empty;
        public DeviceStatusCode DeviceStatusCode { get; set; }
    }
}