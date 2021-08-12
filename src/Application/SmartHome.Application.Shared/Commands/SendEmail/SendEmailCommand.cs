using System;
using MediatR;
using SmartHome.Application.Shared.Events;
using SmartHome.Application.Shared.Interfaces.Command;

namespace SmartHome.Application.Shared.Commands.SendEmail
{
    public class SendEmailCommand : IRequest<CommandResultEvent>, ICommand
    {
        public string Subject { get; set; } = string.Empty;
        public string RecipientEmail { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public Guid CorrelationId { get; set; }
        public string CommandName => nameof(SendEmailCommand);
    }
}