using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SmartHome.Application.Exceptions;

namespace SmartHome.Application.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        private static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions();

        public static async Task<T> Deserialize<T>(this HttpResponseMessage response,
            JsonSerializerOptions? options = null) where T : class
        {
            options ??= DefaultOptions;
            Stream contentStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<T>(contentStream, options);
            return result ?? throw new HttpResponseMessageDeserializationException("Deserialization result is null");
        }

        public static async Task<string> ToStringAsync(this HttpResponseMessage response)
        {
            Stream contentStream = await response.Content.ReadAsStreamAsync();
            return await StreamToStringAsync(contentStream);
        }

        private static async Task<string> StreamToStringAsync(Stream stream)
        {
            using var sr = new StreamReader(stream);
            return await sr.ReadToEndAsync();
        }
    }
}