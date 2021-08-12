using MediatR;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.Rules.GetRuleExecutionHistory
{
    public class GetRuleExecutionHistoryQuery : PageRequest, IRequest<PaginationResult<RuleExecutionHistoryVm>>
    {
        public long RuleId { get; set; }
    }
}