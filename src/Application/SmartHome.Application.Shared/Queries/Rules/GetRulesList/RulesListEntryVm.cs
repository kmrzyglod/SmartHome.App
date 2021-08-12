namespace SmartHome.Application.Shared.Queries.Rules.GetRulesList
{
    public class RulesListEntryVm
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}