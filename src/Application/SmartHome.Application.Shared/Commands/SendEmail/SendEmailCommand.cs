using System;
using MediatR;
using SmartHome.Application.Shared.Interfaces.Command;

namespace SmartHome.Application.Shared.Commands.SendEmail
{
    public class SendEmailCommand : IRequest, ICommand
    {
        public string Topic { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public Guid CorrelationId { get; set; }
        public string CommandName => nameof(SendEmailCommand);
    }
}