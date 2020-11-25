using System.Net.Http;
using SmartHome.Application.Interfaces.HttpClient;

namespace SmartHome.Infrastructure.HttpClients
{
    public class ApiHealthCheckHttpClient : IApiHealthCheckHttpClient
    {
        private readonly HttpClient _client;

        public ApiHealthCheckHttpClient(HttpClient client)
        {
            _client = client;
        }

        public HttpClient Get()
        {
            return _client;
        }
    }
}