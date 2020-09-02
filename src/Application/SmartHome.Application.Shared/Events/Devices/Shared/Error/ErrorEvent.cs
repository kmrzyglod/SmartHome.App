using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Application.Shared.Events.Devices.Shared.Error
{
    public class ErrorEvent: IEvent
    {
        public string Message { get; set; } = string.Empty;
        public ErrorEventLevel ErrorLevel { get; set; }
        public string Source { get; set; } = string.Empty;
    }
}
