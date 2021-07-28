using System.Text.Json;
using SmartHome.Clients.WebApp.Helpers.JsonConverters;

namespace SmartHome.Clients.WebApp.Helpers
{
    internal static class CustomJsonSerializerOptionsProvider
    {
        public static readonly JsonSerializerOptions OptionsForApi = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        public static readonly JsonSerializerOptions OptionsWithCaseInsensitive = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        static CustomJsonSerializerOptionsProvider()
        {
            OptionsForApi.Converters.Add(new EventTypeEnumConverter());
            OptionsForApi.Converters.Add(new WindDirectionEnumConverter());
        }
    }
}