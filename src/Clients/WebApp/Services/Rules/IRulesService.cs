using System.Threading.Tasks;
using SmartHome.Application.Shared.Commands.Rules.AddRule;
using SmartHome.Application.Shared.Commands.Rules.DeleteRule;
using SmartHome.Application.Shared.Commands.Rules.SetRuleState;
using SmartHome.Application.Shared.Commands.Rules.UpdateRule;
using SmartHome.Application.Shared.Models;
using SmartHome.Application.Shared.Queries.Rules.GetRuleDetails;
using SmartHome.Application.Shared.Queries.Rules.GetRuleExecutionHistory;
using SmartHome.Application.Shared.Queries.Rules.GetRulesList;

namespace SmartHome.Clients.WebApp.Services.Rules
{
    public interface IRulesService
    {
        Task<PaginationResult<RulesListEntryVm>> GetRulesList(GetRulesListQuery query, bool withCache = true);

        Task<PaginationResult<RuleExecutionHistoryVm>> GetRuleExecutionHistory(GetRuleExecutionHistoryQuery query,
            bool withCache = true);

        Task<RuleDetailsVm> GetRuleDetails(GetRuleDetailsQuery query, bool withCache = true);
        Task<CommandCorrelationId> AddRule(AddRuleCommand command);
        Task<CommandCorrelationId> UpdateRule(UpdateRuleCommand command);
        Task<CommandCorrelationId> SetRuleState(SetRuleStateCommand command);
        Task<CommandCorrelationId> DeleteRule(DeleteRuleCommand command);
    }
}