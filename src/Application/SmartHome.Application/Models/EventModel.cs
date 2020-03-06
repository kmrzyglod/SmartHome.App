using System;
using MongoDB.Bson;
using SmartHome.Application.Enums;
using SmartHome.Application.Interfaces.Event;

namespace SmartHome.Application.Models
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