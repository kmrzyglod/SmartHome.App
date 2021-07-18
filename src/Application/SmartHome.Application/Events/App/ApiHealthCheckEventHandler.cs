using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartHome.Application.Extensions;
using SmartHome.Application.Interfaces.HttpClient;
using SmartHome.Application.Logging;
using SmartHome.Application.Shared.Events.App;

namespace SmartHome.Application.Events.App
{
    public class ApiHealthCheckEventHandler : INotificationHandler<ApiHealthCheckEvent>
    {
        private readonly IApiHealthCheckHttpClient _client;
        private readonly ILogger<LoggingContext> _logger;

        public ApiHealthCheckEventHandler(IApiHealthCheckHttpClient client, ILogger<LoggingContext> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task Handle(ApiHealthCheckEvent notification, CancellationToken cancellationToken)
        {
            using HttpResponseMessage response = await _client.Get().GetAsync("", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogCritical(
                    $"API health check result: FAILED. Response status code: {(int) response.StatusCode}. Response content: '{await response.ToStringAsync()}'");
                //TODO implement some health check failure logic
            }
            _logger.LogInformation("API health check result: OK");
        }
    }
}