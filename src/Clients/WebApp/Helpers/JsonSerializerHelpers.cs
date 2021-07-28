using System;
using System.Text.Json;

namespace SmartHome.Clients.WebApp.Helpers
{
    public static class JsonSerializerHelpers
    {
        public static T DeserializeFromObject<T>(object obj) where T : class
        {
            var deserialized = JsonSerializer.Deserialize<T>(
                ((JsonElement) obj).GetRawText(), CustomJsonSerializerOptionsProvider.Options)!;

            return deserialized;
        }

        public static object DeserializeFromObject(object obj, Type returnType)
        {
            var deserialized = JsonSerializer.Deserialize(
                ((JsonElement) obj).GetRawText(), returnType, CustomJsonSerializerOptionsProvider.Options)!;

            return deserialized;
        }
    }
}