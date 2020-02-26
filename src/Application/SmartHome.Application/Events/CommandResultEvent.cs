using System;
using SmartHome.Application.Enums;
using SmartHome.Application.Interfaces.Event;

namespace SmartHome.Application.Events
{
    public class CommandResultEvent : ICommandResultEvent
    {
        public Guid CorrelationId { get; set; }
        public StatusCode Status { get; }
        public string ErrorMessage { get; }
        public string Source { get; set; }
    }
}