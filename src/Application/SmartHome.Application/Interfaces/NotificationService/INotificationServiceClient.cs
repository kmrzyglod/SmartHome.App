using System.Threading;
using System.Threading.Tasks;
using SmartHome.Application.Shared.Models.NotificationService;

namespace SmartHome.Application.Interfaces.NotificationService
{
    public interface INotificationServiceClient
    {
        Task<NegotiateResult> Negotiate(CancellationToken cancellationToken);
    }
}