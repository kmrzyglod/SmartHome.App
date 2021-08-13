using System;
using SmartHome.Domain.Entities.Rules;

namespace SmartHome.Application.Shared.Queries.Rules.GetRulesExecutionHistoryList
{
    public class RulesExecutionHistoryListVm
    {
        public long Id { get; set; }
        public long RuleId { get; set; }
        public string RuleName { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public RuleExecutionStatus ExecutionStatus { get; set; }
    }
}