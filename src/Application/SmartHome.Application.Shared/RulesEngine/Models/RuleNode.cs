using System.Collections.Generic;
using System.Linq;

namespace SmartHome.Application.Shared.RulesEngine.Models
{
    public class RuleNode: RuleDefinition
    {
        public RuleCondition condition { get; set; }
        public IEnumerable<RuleNode> rules { get; set; } = Enumerable.Empty<RuleNode>();
    }
}
