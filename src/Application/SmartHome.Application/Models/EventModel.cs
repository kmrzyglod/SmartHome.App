using System;
using SmartHome.Application.Enums;
using SmartHome.Application.Interfaces.Event;

namespace SmartHome.Application.Models
{
    public class EventModel
    {
        public DateTime Timestamp { get; set; }
        public string EventName { get; set; } = string.Empty;
        public EventType EventType { get; set; }
        public IEvent? EventData { get; set; }
    }
}