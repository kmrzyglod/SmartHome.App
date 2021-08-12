using System;

namespace SmartHome.Domain.Entities.Rules
{
    public class RuleExecutionHistory
    {
        public long Id { get; set; }
        public long RuleId { get; set; }
        public Rule Rule { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public RuleExecutionStatus ExecutionStatus { get; set; }
        public string? ErrorMessage { get; set; }
    }
}