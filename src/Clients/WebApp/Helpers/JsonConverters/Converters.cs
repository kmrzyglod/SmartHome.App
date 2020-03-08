using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Interfaces.Event;
using SmartHome.Domain.Enums;

namespace SmartHome.Clients.WebApp.Helpers.JsonConverters
{
    internal class WindDirectionEnumConverter : JsonConverter<WindDirection>
    {
        public override WindDirection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) 
            => (WindDirection)Enum.Parse(typeof(WindDirection), reader.GetString());

        public override void Write(Utf8JsonWriter writer, WindDirection value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString());
    }

    internal class EventTypeEnumConverter : JsonConverter<EventType>
    {
        public override EventType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) 
            => (EventType)Enum.Parse(typeof(EventType), reader.GetString());

        public override void Write(Utf8JsonWriter writer, EventType value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString());
    }
}
