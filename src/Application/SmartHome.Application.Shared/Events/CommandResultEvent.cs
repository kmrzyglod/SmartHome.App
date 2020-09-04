using System;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Application.Shared.Events
{
    public class CommandResultEvent : EventBase, ICommandResultEvent
    {
        public Guid CorrelationId { get; set; }
        public StatusCode Status { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string CommandName { get; set; } = string.Empty;
    }
}