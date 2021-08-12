using System.Text.Json;

namespace SmartHome.Application.Shared.Helpers.JsonHelpers
{
    public static class CustomJsonSerializerOptionsProvider
    {
        public static readonly JsonSerializerOptions OptionsEnumConverter = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            IgnoreNullValues = true,
            Converters = { new CustomJsonStringEnumConverter()}
        };

        public static readonly JsonSerializerOptions OptionsForApi = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreNullValues = true,
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
            OptionsForApi.Converters.Add(new CustomJsonStringEnumConverter());
        }
    }
}