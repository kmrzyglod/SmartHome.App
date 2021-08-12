using SmartHome.Application.Shared.RulesEngine.Models;

namespace SmartHome.Application.Shared.Queries.Rules.GetRuleDetails
{
    public class RuleDetailsVm
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public RuleNode Body { get; set; } = new RuleNode();
        public RuleOutputAction OutputAction { get; set; } = new RuleOutputAction();
    }
}