using System;
using MediatR;
using SmartHome.Application.Shared.Events;
using SmartHome.Application.Shared.Interfaces.Command;

namespace SmartHome.Application.Shared.Commands.Rules.SetRuleState
{
    public class SetRuleStateCommand : IRequest<CommandResultEvent>, ICommand
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
        public Guid CorrelationId { get; set; }
        public string CommandName => nameof(SetRuleStateCommand);
    }
}