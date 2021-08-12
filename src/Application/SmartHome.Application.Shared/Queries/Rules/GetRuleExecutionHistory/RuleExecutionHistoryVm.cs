using System;
using SmartHome.Domain.Entities.Rules;

namespace SmartHome.Application.Shared.Queries.Rules.GetRuleExecutionHistory
{
    public class RuleExecutionHistoryVm
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }
        public RuleExecutionStatus ExecutionStatus { get; set; }
    }
}