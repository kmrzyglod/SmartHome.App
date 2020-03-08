﻿using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartHome.Clients.WebApp.Helpers
{
     public static class CustomHttpClientJsonExtensions
    {
        /// <summary>
        /// Sends a GET request to the specified URI, and parses the JSON response body
        /// to create an object of the generic type.
        /// </summary>
        /// <typeparam name="T">A type into which the response body can be JSON-deserialized.</typeparam>
        /// <param name="httpClient">The <see cref="HttpClient"/>.</param>
        /// <param name="requestUri">The URI that the request will be sent to.</param>
        /// <returns>The response parsed as an object of the generic type.</returns>
        public static async Task<T> GetCustomJsonAsync<T>(this HttpClient httpClient, string requestUri)
        {
            var stringContent = await httpClient.GetStringAsync(requestUri);
            return JsonSerializer.Deserialize<T>(stringContent, CustomJsonSerializerOptionsProvider.Options);
        }

        /// <summary>
        /// Sends a POST request to the specified URI, including the specified <paramref name="content"/>
        /// in JSON-encoded format, and parses the JSON response body to create an object of the generic type.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/>.</param>
        /// <param name="requestUri">The URI that the request will be sent to.</param>
        /// <param name="content">Content for the request body. This will be JSON-encoded and sent as a string.</param>
        /// <returns>The response parsed as an object of the generic type.</returns>
        public static Task PostCustomJsonAsync(this HttpClient httpClient, string requestUri, object content)
            => httpClient.SendCustomJsonAsync(HttpMethod.Post, requestUri, content);

        /// <summary>
        /// Sends a POST request to the specified URI, including the specified <paramref name="content"/>
        /// in JSON-encoded format, and parses the JSON response body to create an object of the generic type.
        /// </summary>
        /// <typeparam name="T">A type into which the response body can be JSON-deserialized.</typeparam>
        /// <param name="httpClient">The <see cref="HttpClient"/>.</param>
        /// <param name="requestUri">The URI that the request will be sent to.</param>
        /// <param name="content">Content for the request body. This will be JSON-encoded and sent as a string.</param>
        /// <returns>The response parsed as an object of the generic type.</returns>
        public static Task<T> PostCustomJsonAsync<T>(this HttpClient httpClient, string requestUri, object content)
            => httpClient.SendCustomJsonAsync<T>(HttpMethod.Post, requestUri, content);

        /// <summary>
        /// Sends a PUT request to the specified URI, including the specified <paramref name="content"/>
        /// in JSON-encoded format.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/>.</param>
        /// <param name="requestUri">The URI that the request will be sent to.</param>
        /// <param name="content">Content for the request body. This will be JSON-encoded and sent as a string.</param>
        public static Task PutCustomJsonAsync(this HttpClient httpClient, string requestUri, object content)
            => httpClient.SendCustomJsonAsync(HttpMethod.Put, requestUri, content);

        /// <summary>
        /// Sends a PUT request to the specified URI, including the specified <paramref name="content"/>
        /// in JSON-encoded format, and parses the JSON response body to create an object of the generic type.
        /// </summary>
        /// <typeparam name="T">A type into which the response body can be JSON-deserialized.</typeparam>
        /// <param name="httpClient">The <see cref="HttpClient"/>.</param>
        /// <param name="requestUri">The URI that the request will be sent to.</param>
        /// <param name="content">Content for the request body. This will be JSON-encoded and sent as a string.</param>
        /// <returns>The response parsed as an object of the generic type.</returns>
        public static Task<T> PutCustomJsonAsync<T>(this HttpClient httpClient, string requestUri, object content)
            => httpClient.SendCustomJsonAsync<T>(HttpMethod.Put, requestUri, content);

        /// <summary>
        /// Sends an HTTP request to the specified URI, including the specified <paramref name="content"/>
        /// in JSON-encoded format.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/>.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="requestUri">The URI that the request will be sent to.</param>
        /// <param name="content">Content for the request body. This will be JSON-encoded and sent as a string.</param>
        public static Task SendCustomJsonAsync(this HttpClient httpClient, HttpMethod method, string requestUri, object content)
            => httpClient.SendCustomJsonAsync<IgnoreResponse>(method, requestUri, content);

        /// <summary>
        /// Sends an HTTP request to the specified URI, including the specified <paramref name="content"/>
        /// in JSON-encoded format, and parses the JSON response body to create an object of the generic type.
        /// </summary>
        /// <typeparam name="T">A type into which the response body can be JSON-deserialized.</typeparam>
        /// <param name="httpClient">The <see cref="HttpClient"/>.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="requestUri">The URI that the request will be sent to.</param>
        /// <param name="content">Content for the request body. This will be JSON-encoded and sent as a string.</param>
        /// <returns>The response parsed as an object of the generic type.</returns>
        public static async Task<T> SendCustomJsonAsync<T>(this HttpClient httpClient, HttpMethod method, string requestUri, object content)
        {
            var requestJson = JsonSerializer.Serialize(content, CustomJsonSerializerOptionsProvider.Options);
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
            else
            {
                var stringContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(stringContent, CustomJsonSerializerOptionsProvider.Options);
            }
        }

        class IgnoreResponse { }
    }
}
