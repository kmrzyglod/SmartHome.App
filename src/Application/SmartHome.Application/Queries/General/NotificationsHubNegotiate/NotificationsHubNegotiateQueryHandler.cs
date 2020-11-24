using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHome.Application.Interfaces.NotificationService;
using SmartHome.Application.Shared.Queries.General.NotificationsHubNegotiate;

namespace SmartHome.Application.Queries.General.NotificationsHubNegotiate
{
    public class
        NotificationsHubNegotiateQueryHandler : IRequestHandler<NotificationsHubNegotiateQuery, NegotiateResultVm>
    {
        private readonly INotificationServiceClient _notificationServiceClient;

        public NotificationsHubNegotiateQueryHandler(INotificationServiceClient notificationServiceClient) =>
            _notificationServiceClient = notificationServiceClient;

        public async Task<NegotiateResultVm> Handle(NotificationsHubNegotiateQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _notificationServiceClient.Negotiate(cancellationToken);
            return new NegotiateResultVm
            {
                accessToken = result.accessToken,
                url = result.url
            };
        }
    }
}