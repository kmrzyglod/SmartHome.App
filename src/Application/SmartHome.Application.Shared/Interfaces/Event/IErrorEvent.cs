using SmartHome.Application.Shared.Enums;

namespace SmartHome.Application.Shared.Interfaces.Event
{
    public interface IErrorEvent: IEvent
    {
        string Message { get; set; }
        ErrorEventLevel ErrorLevel { get; set; }
    }
}