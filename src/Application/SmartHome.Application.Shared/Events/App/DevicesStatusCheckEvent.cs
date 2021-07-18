using MediatR;
using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Application.Shared.Events.App
{
    public class DevicesStatusCheckEvent: INotification, IScheduledTask
    {

    }
}
