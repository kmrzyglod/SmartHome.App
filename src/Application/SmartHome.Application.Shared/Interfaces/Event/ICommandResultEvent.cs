using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Interfaces.Message;

namespace SmartHome.Application.Shared.Interfaces.Event
{
    public interface ICommandResultEvent : IEvent, IMessage
    {
        StatusCode Status { get; set;}
        string ErrorMessage { get; set; }
        string CommandName { get; set;}
    }
}