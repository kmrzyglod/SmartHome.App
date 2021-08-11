using MediatR;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.Rules.GetRulesList
{
    public class GetRulesListQuery:  PageRequest, IRequest<PaginationResult<RulesListEntryVm>>
    {
    }
}
