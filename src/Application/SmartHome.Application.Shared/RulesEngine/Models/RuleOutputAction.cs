using System.Collections.Generic;
using SmartHome.Application.Shared.Interfaces.Command;

namespace SmartHome.Application.Shared.RulesEngine.Models
{
    public class RuleOutputAction
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public IEnumerable<ICommand> Commands { get; set; } = new List<ICommand>();
    }
}