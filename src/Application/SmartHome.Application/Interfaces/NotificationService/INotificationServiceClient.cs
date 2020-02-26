using System.Threading.Tasks;
using SmartHome.Application.Interfaces.Event;

namespace SmartHome.Application.Interfaces.NotificationService
{
    public interface INotificationServiceClient
    {
        Task SendNotificationAsync(IEvent @event);
    }
}