using SmartHome.Application.Shared.Enums;

namespace SmartHome.Application.Shared.Events.Devices.Shared.Error
{
    public class ErrorEvent: EventBase
    {
        public string Message { get; set; } = string.Empty;
        public ErrorEventLevel ErrorLevel { get; set; }
    }
}
