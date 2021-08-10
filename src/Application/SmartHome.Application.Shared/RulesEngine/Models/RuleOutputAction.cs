using System.Collections.Generic;
using SmartHome.Application.Shared.Interfaces.Command;

namespace SmartHome.Application.Shared.RulesEngine.Models
{
    public class RuleOutputAction
    {
        public RuleOutputActionId Id { get; set; } = default;
        public string Name { get; set; } = string.Empty;
        public IEnumerable<ICommand> Commands { get; set; } = new List<ICommand>();
    }
}