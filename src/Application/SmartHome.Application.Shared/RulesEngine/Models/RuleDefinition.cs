namespace SmartHome.Application.Shared.RulesEngine.Models
{
    public abstract class RuleDefinition
    {
        public string? id { get; set; }
        public string? field { get; set; } 
        public string? type { get; set; }
        public string? input { get; set; } 
        public string? @operator { get; set; }
        public object? value { get; set; }
    }
}