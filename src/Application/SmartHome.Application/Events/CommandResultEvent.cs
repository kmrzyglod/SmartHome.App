using System;
using SmartHome.Application.Enums;
using SmartHome.Application.Interfaces.Event;

namespace SmartHome.Application.Events
{
    public class CommandResultEvent : ICommandResultEvent
    {
        public Guid CorrelationId { get; set; }
        public StatusCode Status { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string CommandName { get; } = string.Empty;
        public string Source { get; set; } = string.Empty;
    }
}