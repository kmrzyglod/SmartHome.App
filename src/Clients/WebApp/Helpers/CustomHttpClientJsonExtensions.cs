using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartHome.Clients.WebApp.Helpers
{
    public static class CustomHttpClientJsonExtensions
    {
        /// <summary>
        ///     Sends a GET request to the specified URI, and parses the JSON response body
        ///     to create an object of the generic type.
        /// </summary>
        /// <typeparam name="T">A type into which the response body can be JSON-deserialized.</typeparam>
        /// <param name="httpClient">The <see cref="HttpClient" />.</param>
        /// <param name="requestUri">The URI that the request will be sent to.</param>
        /// <param name="customHeaders"></param>
        /// <returns>The response parsed as an object of the generic type.</returns>
        public static async Task<T?> GetCustomJsonAsync<T>(this HttpClient httpClient, string requestUri,
            IDictionary<string, IEnumerable<string>>? customHeaders = null) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            if (customHeaders != null && customHeaders.Any())
            {
                foreach (var (headerName, headerValue) in customHeaders)
                {
                    request.Headers.Add(headerName, headerValue);
                }
            }

            using var result = await httpClient.SendAsync(request);
            string? stringContent = await result.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(stringContent)
                ? null
                : JsonSerializer.Deserialize<T>(stringContent, CustomJsonSerializerOptionsProvider.OptionsForApi);
        }

        /// <summary>
        ///     Sends a POST request to the specified URI, including the specified <paramref name="content" />
        ///     in JSON-encoded format, and parses the JSON response body to create an object of the generic type.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient" />.</param>
        /// <param name="requestUri">The URI that the request will be sent to.</param>
        /// <param name="content">Content for the request body. This will be JSON-encoded and sent as a string.</param>
        /// <returns>The response parsed as an object of the generic type.</returns>
        public static Task PostCustomJsonAsync(this HttpClient httpClient, string requestUri, object content)
        {
            return httpClient.SendCustomJsonAsync(HttpMethod.Post, requestUri, content);
        }

        /// <summary>
        ///     Sends a POST request to the specified URI, including the specified <paramref name="content" />
        ///     in JSON-encoded format, and parses the JSON response body to create an object of the generic type.
        /// </summary>
        /// <typeparam name="T">A type into which the response body can be JSON-deserialized.</typeparam>
        /// <param name="httpClient">The <see cref="HttpClient" />.</param>
        /// <param name="requestUri">The URI that the request will be sent to.</param>
        /// <param name="content">Content for the request body. This will be JSON-encoded and sent as a string.</param>
        /// <returns>The response parsed as an object of the generic type.</returns>
        public static Task<T> PostCustomJsonAsync<T>(this HttpClient httpClient, string requestUri, object content)
        {
            return httpClient.SendCustomJsonAsync<T>(HttpMethod.Post, requestUri, content);
        }

        /// <summary>
        ///     Sends a PUT request to the specified URI, including the specified <paramref name="content" />
        ///     in JSON-encoded format.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient" />.</param>
        /// <param name="requestUri">The URI that the request will be sent to.</param>
        /// <param name="content">Content for the request body. This will be JSON-encoded and sent as a string.</param>
        public static Task PutCustomJsonAsync(this HttpClient httpClient, string requestUri, object content)
        {
            return httpClient.SendCustomJsonAsync(HttpMethod.Put, requestUri, content);
        }

        /// <summary>
        ///     Sends a PUT request to the specified URI, including the specified <paramref name="content" />
        ///     in JSON-encoded format, and parses the JSON response body to create an object of the generic type.
        /// </summary>
        /// <typeparam name="T">A type into which the response body can be JSON-deserialized.</typeparam>
        /// <param name="httpClient">The <see cref="HttpClient" />.</param>
        /// <param name="requestUri">The URI that the request will be sent to.</param>
        /// <param name="content">Content for the request body. This will be JSON-encoded and sent as a string.</param>
        /// <returns>The response parsed as an object of the generic type.</returns>
        public static Task<T> PutCustomJsonAsync<T>(this HttpClient httpClient, string requestUri, object content)
        {
            return httpClient.SendCustomJsonAsync<T>(HttpMethod.Put, requestUri, content);
        }

        /// <summary>
        ///     Sends an HTTP request to the specified URI, including the specified <paramref name="content" />
        ///     in JSON-encoded format.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient" />.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="requestUri">The URI that the request will be sent to.</param>
        /// <param name="content">Content for the request body. This will be JSON-encoded and sent as a string.</param>
        public static Task SendCustomJsonAsync(this HttpClient httpClient, HttpMethod method, string requestUri,
            object content)
        {
            return httpClient.SendCustomJsonAsync<IgnoreResponse>(method, requestUri, content);
        }

        /// <summary>
        ///     Sends an HTTP request to the specified URI, including the specified <paramref name="content" />
        ///     in JSON-encoded format, and parses the JSON response body to create an object of the generic type.
        /// </summary>
        /// <typeparam name="T">A type into which the response body can be JSON-deserialized.</typeparam>
        /// <param name="httpClient">The <see cref="HttpClient" />.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="requestUri">The URI that the request will be sent to.</param>
        /// <param name="content">Content for the request body. This will be JSON-encoded and sent as a string.</param>
        /// <returns>The response parsed as an object of the generic type.</returns>
        public static async Task<T> SendCustomJsonAsync<T>(this HttpClient httpClient, HttpMethod method,
            string requestUri, object content)
        {
            string? requestJson = JsonSerializer.Serialize(content, CustomJsonSerializerOptionsProvider.OptionsForApi);
            var response = await httpClient.SendAsync(new HttpRequestMessage(method, requestUri)
            {
                Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
            });

            // Make sure the call was successful before we
            // attempt to process the response content
            response.EnsureSuccessStatusCode();

            if (typeof(T) == typeof(IgnoreResponse))
            {
                return default;
            }

            string? stringContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(stringContent, CustomJsonSerializerOptionsProvider.OptionsForApi);
        }

        private class IgnoreResponse
        {
        }
    }
}