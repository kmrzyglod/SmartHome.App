﻿using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartHome.Application.Extensions;
using SmartHome.Application.Interfaces.HttpClient;
using SmartHome.Application.Shared.Events.App;

namespace SmartHome.Application.Events.App
{
    public class ApiHealthCheckEventHandler : INotificationHandler<HealthCheckEvent>
    {
        private readonly IApiHealthCheckHttpClient _client;
        private readonly ILogger<ApiHealthCheckEventHandler> _logger;

        public ApiHealthCheckEventHandler(IApiHealthCheckHttpClient client, ILogger<ApiHealthCheckEventHandler> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task Handle(HealthCheckEvent notification, CancellationToken cancellationToken)
        {
            using HttpResponseMessage response = await _client.Get().PostAsync("", null, cancellationToken);
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