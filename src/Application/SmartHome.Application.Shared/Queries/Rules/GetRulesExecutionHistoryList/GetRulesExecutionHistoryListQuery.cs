using MediatR;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.Rules.GetRulesExecutionHistoryList
{
    public class GetRulesExecutionHistoryListQuery : PageRequest,
        IRequest<PaginationResult<RulesExecutionHistoryListVm>>
    {
    }
}