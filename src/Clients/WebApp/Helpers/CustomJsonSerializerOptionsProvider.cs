using System.Text.Json;
using SmartHome.Clients.WebApp.Helpers.JsonConverters;

namespace SmartHome.Clients.WebApp.Helpers
{
    internal static class CustomJsonSerializerOptionsProvider
    {
        public static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        static CustomJsonSerializerOptionsProvider()
        {
            Options.Converters.Add(new EventTypeEnumConverter());
            Options.Converters.Add(new WindDirectionEnumConverter());
        }
    }
}