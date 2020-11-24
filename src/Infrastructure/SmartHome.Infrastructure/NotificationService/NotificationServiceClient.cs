using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SmartHome.Application.Exceptions;
using SmartHome.Application.Extensions;
using SmartHome.Application.Interfaces.NotificationService;
using SmartHome.Application.Shared.Models.NotificationService;

namespace SmartHome.Infrastructure.NotificationService
{
    public class NotificationServiceClient : INotificationServiceClient
    {
        private readonly HttpClient _client;

        public NotificationServiceClient(HttpClient client) => _client = client;

        public async Task<NegotiateResult> Negotiate(CancellationToken cancellationToken)
        {
            using HttpResponseMessage response = await _client.PostAsync("NotificationsHubNegotiate", null, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Deserialize<NegotiateResult>();
            }

            throw new NotificationServiceClientException
            {
                StatusCode = (int) response.StatusCode,
                Content = await response.ToStringAsync()
            };
        }
    }
}