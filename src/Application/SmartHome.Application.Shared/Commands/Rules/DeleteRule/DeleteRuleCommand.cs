using System;
using MediatR;
using SmartHome.Application.Shared.Events;
using SmartHome.Application.Shared.Interfaces.Command;

namespace SmartHome.Application.Shared.Commands.Rules.DeleteRule
{
    public class DeleteRuleCommand : IRequest<CommandResultEvent>, ICommand
    {
        public long Id { get; set; }
        public Guid CorrelationId { get; set; }
        public string CommandName => nameof(DeleteRuleCommand);
    }
}