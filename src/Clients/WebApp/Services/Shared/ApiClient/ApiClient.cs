using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SmartHome.Application.Shared.Helpers.JsonHelpers;
using SmartHome.Clients.WebApp.Helpers;

namespace SmartHome.Clients.WebApp.Services.Shared.ApiClient
{
    public class ApiClient : IApiClient
    {
        //private const string API_URL = "https://km-smart-home-api.azurewebsites.net/api/v1/";
        private const string API_URL = "https://localhost:44374/api/v1/";

        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(API_URL);
        }

        public IDictionary<string, IEnumerable<string>>? NoCacheHeader { get; } =
            new Dictionary<string, IEnumerable<string>>
            {
                {"Cache-Control", new List<string> {"no-cache"}}
            };

        public Task<TResponse?> Get<TResponse>(string url,
            IDictionary<string, IEnumerable<string>>? customHeaders = null
        ) where TResponse : class
        {
            return _httpClient.GetCustomJsonAsync<TResponse>(url, customHeaders);
        }

        public Task<TResponse?> Get<TQuery, TResponse>(string url, TQuery query,
            IDictionary<string, IEnumerable<string>>? customHeaders = null) where TQuery : class where TResponse : class
        {
            return _httpClient.GetCustomJsonAsync<TResponse>(GetUrlWithQueryParameters(url, query), customHeaders);
        }

        public Task<TResponse> Post<TRequest, TResponse>(string url, TRequest request) where TRequest : class
        {
            return _httpClient.PostCustomJsonAsync<TResponse>(url, request);
        }

        public Task Post<TRequest>(string url, TRequest request) where TRequest : class
        {
            return _httpClient.PostCustomJsonAsync(url, request);
        }

        public Task<TResponse> Put<TRequest, TResponse>(string url, TRequest request) where TRequest : class
        {
            return _httpClient.PutCustomJsonAsync<TResponse>(url, request);
        }

        public Task Put<TRequest>(string url, TRequest request) where TRequest : class
        {
            return _httpClient.PutCustomJsonAsync(url, request);
        }

        public Task<TResponse> Patch<TRequest, TResponse>(string url, TRequest request) where TRequest : class
        {
            return _httpClient.PatchCustomJsonAsync<TResponse>(url, request);
        }

        public Task Patch<TRequest>(string url, TRequest request) where TRequest : class
        {
            return _httpClient.PatchCustomJsonAsync(url, request);
        }

        public Task Delete(string url)
        {
            return _httpClient.DeleteAsync(url);
        }

        public Task Delete<TQuery>(string url, TQuery query) where TQuery : class
        {
            return _httpClient.DeleteAsync(GetUrlWithQueryParameters(url, query));
        }

        public async Task<TResponse> Delete<TRequest, TResponse>(string url, TRequest request) where TRequest : class
        {
            var response =  await _httpClient.DeleteAsync(GetUrlWithQueryParameters(url, request));
            string? stringContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(stringContent, CustomJsonSerializerOptionsProvider.OptionsForApi);
        }

        private string GetUrlWithQueryParameters<TQuery>(string url, TQuery query) where TQuery : class
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"{url}?");
            foreach (var propertyInfo in query.GetType().GetProperties())
            {
                string? parameterValue = ConvertProperty(propertyInfo, propertyInfo.GetValue(query));

                if (parameterValue == null)
                {
                    continue;
                }

                stringBuilder.Append($"{propertyInfo.Name}={parameterValue}&");
            }

            stringBuilder.Length--;

            return stringBuilder.ToString();
        }

        private string? ConvertProperty(PropertyInfo propertyInfo, object value)
        {
            if (propertyInfo.PropertyType == typeof(DateTime))
            {
                return ((DateTime) value).ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ");
            }

            if (propertyInfo.PropertyType == typeof(DateTime?) && value != null)
            {
                return ((DateTime?) value).Value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ");
            }

            return value?.ToString();
        }
    }
}