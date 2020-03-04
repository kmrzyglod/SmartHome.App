using SmartHome.Application.Enums;
using SmartHome.Application.Interfaces.Event;

namespace SmartHome.Application.Events.Devices.Shared.DeviceStatusUpdated
{
    public class DeviceStatusUpdatedEvent : IEvent
    {
        public string Message { get; set; } = string.Empty;
        public DeviceStatusCode DeviceStatusCode { get; set; }
        public string Source { get; set; } = string.Empty;
    }
}