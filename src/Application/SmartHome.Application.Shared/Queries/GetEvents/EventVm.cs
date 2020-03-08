using System;
using SmartHome.Application.Shared.Enums;

namespace SmartHome.Application.Shared.Queries.GetEvents
{
    public class EventVm
    {
        public string Id { get; set; } = string.Empty;
        public EventType EventType { get; set; }
        public string EventName { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public object? EventData { get; set; }
        public string Source { get; set; } = string.Empty;
    }
}
