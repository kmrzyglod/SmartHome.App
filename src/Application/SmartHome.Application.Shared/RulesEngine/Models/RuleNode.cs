using System.Collections.Generic;

namespace SmartHome.Application.Shared.RulesEngine.Models
{
    public class RuleNode: RuleDefinition
    {
        public string? condition { get; set; }
        public IEnumerable<RuleNode>? rules { get; set; }
        public bool? valid { get; set; }
    }
}
