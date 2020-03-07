using System.Threading.Tasks;
using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Application.Interfaces.NotificationService
{
    public interface INotificationServiceClient
    {
        Task SendNotificationAsync(IEvent @event);
    }
}