using System;
using MongoDB.Bson;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Application.Shared.Models
{
    public class EventModel
    {
        public ObjectId Id { get; set; } 
        public EventType EventType { get; set; }
        public string EventName { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public IEvent? EventData { get; set; }
        public string Source { get; set; } = string.Empty;
    }
}