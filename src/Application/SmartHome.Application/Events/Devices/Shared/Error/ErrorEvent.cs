using SmartHome.Application.Enums;
using SmartHome.Application.Interfaces.Event;

namespace SmartHome.Application.Events.Devices.Shared.Error
{
    public class ErrorEvent: IEvent
    {
        public string Message { get; set; } = string.Empty;
        public ErrorEventLevel ErrorLevel { get; set; }
        public string Source { get; set; } = string.Empty;
    }
}
