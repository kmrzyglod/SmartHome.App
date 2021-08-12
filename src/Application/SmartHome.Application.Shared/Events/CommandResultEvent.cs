using System;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Interfaces.Command;
using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Application.Shared.Events
{
    public class CommandResultEvent : EventBase, ICommandResultEvent
    {
        public Guid CorrelationId { get; set; }
        public StatusCode Status { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string CommandName { get; set; } = string.Empty;

        public static CommandResultEvent CreateAppCommandResult<T>(T command, StatusCode statusCode, DateTime timestamp,
            string? errorMessage = null) where T : ICommand
        {
            return new CommandResultEvent
            {
                Source = "App",
                Timestamp = timestamp,
                CorrelationId = command.CorrelationId,
                Status = statusCode,
                ErrorMessage = errorMessage ?? string.Empty,
                CommandName = typeof(T).Name
            };
        }
    }
}