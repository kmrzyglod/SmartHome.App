﻿using System;
using System.Text.Json;

namespace SmartHome.Clients.WebApp.Helpers
{
    public static class JsonSerializerHelpers
    {
        public static T DeserializeFromObject<T>(object obj, JsonSerializerOptions? customSerializationOptions = null) where T : class
        {
            var deserialized = JsonSerializer.Deserialize<T>(
                ((JsonElement) obj).GetRawText(), customSerializationOptions ?? CustomJsonSerializerOptionsProvider.OptionsForApi)!;

            return deserialized;
        }

        public static object DeserializeFromObject(object obj, Type returnType, JsonSerializerOptions? customSerializationOptions = null)
        {
            var deserialized = JsonSerializer.Deserialize(
                ((JsonElement) obj).GetRawText(), returnType, customSerializationOptions ?? CustomJsonSerializerOptionsProvider.OptionsForApi)!;

            return deserialized;
        }
    }
}