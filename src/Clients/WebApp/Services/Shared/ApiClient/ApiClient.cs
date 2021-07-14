using System;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SmartHome.Clients.WebApp.Helpers;

namespace SmartHome.Clients.WebApp.Services.Shared.ApiClient
{
    public class ApiClient : IApiClient
    {
        private const string API_URL = "https://localhost:5001/api/v1/";

        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(API_URL);
        }

        public Task<TResponse?> Get<TResponse>(string url) where TResponse: class
        {
            return _httpClient.GetCustomJsonAsync<TResponse>(url);
        }

        public Task<TResponse?> Get<TQuery, TResponse>(string url, TQuery query) where TQuery : class where TResponse: class
        {
            return _httpClient.GetCustomJsonAsync<TResponse>(GetUrlWithQueryParameters(url, query));
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

        public Task Delete(string url)
        {
            return _httpClient.DeleteAsync(url);
        }

        public Task Delete<TQuery>(string url, TQuery query) where TQuery : class
        {
            return _httpClient.DeleteAsync(GetUrlWithQueryParameters(url, query));
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