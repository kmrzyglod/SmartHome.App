using SmartHome.Application.Enums;

namespace SmartHome.Application.Interfaces.Event
{
    public interface IErrorEvent: IEvent
    {
        string Message { get; set; }
        ErrorEventLevel ErrorLevel { get; set; }
    }
}