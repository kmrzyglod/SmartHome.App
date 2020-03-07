using MediatR;

namespace SmartHome.Application.Shared.Interfaces.Event
{
    public interface IEvent: INotification
    {
        string Source { get; set; }
    }
}