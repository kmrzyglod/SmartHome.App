using System;
using MediatR;
using SmartHome.Application.Shared.Events;
using SmartHome.Application.Shared.Interfaces.Command;
using SmartHome.Application.Shared.RulesEngine.Models;

namespace SmartHome.Application.Shared.Commands.Rules.UpdateRule
{
    public class UpdateRuleCommand : IRequest<CommandResultEvent>, ICommand
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public RuleNode Body { get; set; } = new RuleNode();
        public RuleOutputAction OutputAction { get; set; } = new RuleOutputAction();
        public Guid CorrelationId { get; set; }
        public string CommandName => nameof(UpdateRuleCommand);
    }
}