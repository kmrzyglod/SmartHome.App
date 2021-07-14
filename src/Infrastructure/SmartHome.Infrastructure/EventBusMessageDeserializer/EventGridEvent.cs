using System;
using System.Collections.Generic;

namespace SmartHome.Infrastructure.EventBusMessageDeserializer
{
    public class EventGridEvent
    {
        public string Id { get; set; } = string.Empty;

        public string Topic { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string EventType { get; set; } = string.Empty;

        public DateTime EventTime { get; set; } = default;

        public IDictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
    }
}