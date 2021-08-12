using System.Collections.Generic;

namespace SmartHome.Domain.Entities.Rules
{
    public class Rule
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string Body { get; set; } = string.Empty;
        public string OutputAction { get; set; } = string.Empty;

        public ICollection<RuleExecutionHistory> ExecutionHistory { get; set; } = new List<RuleExecutionHistory>();
    }
}