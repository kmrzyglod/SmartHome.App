namespace SmartHome.Application.Shared.Queries.Rules.GetRuleDetails
{
    public class RuleDetailsVm
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string Body { get; set; } = string.Empty;
        public string OutputAction { get; set; } = string.Empty;
    }
}