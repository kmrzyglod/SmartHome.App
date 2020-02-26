using MediatR;

namespace SmartHome.Application.Interfaces.Event
{
    public interface IEvent: INotification
    {
        string Source { get; set; }
    }
}