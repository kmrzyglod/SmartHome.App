namespace SmartHome.Application.Shared.RulesEngine.Models
{
    public abstract class RuleDefinition
    {
        public RuleId id { get; set; }
        public RuleType type { get; set; }
        public RuleOperator @operator { get; set; }
        public object? value { get; set; }
    }
}