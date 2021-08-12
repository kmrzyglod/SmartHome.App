using System;
using SmartHome.Application.Shared.Interfaces.Event;
using SmartHome.Domain.Entities.Rules;

namespace SmartHome.Application.Shared.Events.App
{
    public class RuleExecutionResultEvent : IEvent
    {
        public long RuleId { get; set; }
        public RuleExecutionStatus ExecutionStatus { get; set; }
        public string? ErrorMessage { get; set; }
        public string Source { get; set; } = "App";
        public DateTime Timestamp { get; set; }
    }
}