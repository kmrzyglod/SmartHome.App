using System.Threading.Tasks;
using SmartHome.Application.Shared.Commands.Rules.AddRule;
using SmartHome.Application.Shared.Commands.Rules.DeleteRule;
using SmartHome.Application.Shared.Commands.Rules.SetRuleState;
using SmartHome.Application.Shared.Commands.Rules.UpdateRule;
using SmartHome.Application.Shared.Models;
using SmartHome.Application.Shared.Queries.Rules.GetRuleDetails;
using SmartHome.Application.Shared.Queries.Rules.GetRuleExecutionHistory;
using SmartHome.Application.Shared.Queries.Rules.GetRulesList;
using SmartHome.Clients.WebApp.Services.Shared.ApiClient;

namespace SmartHome.Clients.WebApp.Services.Rules
{
    public class RulesService : IRulesService
    {
        private readonly IApiClient _apiClient;

        public RulesService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public Task<PaginationResult<RulesListEntryVm>> GetRulesList(GetRulesListQuery query, bool withCache = true)
        {
            return _apiClient.Get<GetRulesListQuery, PaginationResult<RulesListEntryVm>>("Rules/list", query,
                withCache ? null : _apiClient.NoCacheHeader);
        }

        public Task<PaginationResult<RuleExecutionHistoryVm>> GetRuleExecutionHistory(
            GetRuleExecutionHistoryQuery query, bool withCache = true)
        {
            return _apiClient.Get<GetRuleExecutionHistoryQuery, PaginationResult<RuleExecutionHistoryVm>>(
                "Rules/execution-history", query,
                withCache ? null : _apiClient.NoCacheHeader);
        }

        public Task<RuleDetailsVm> GetRuleDetails(GetRuleDetailsQuery query, bool withCache = true)
        {
            return _apiClient.Get<GetRuleDetailsQuery, RuleDetailsVm>("Rules/details", query,
                withCache ? null : _apiClient.NoCacheHeader);
        }

        public Task<CommandCorrelationId> AddRule(AddRuleCommand command)
        {
            return _apiClient.Post<AddRuleCommand, CommandCorrelationId>("Rules", command);
        }

        public Task<CommandCorrelationId> UpdateRule(UpdateRuleCommand command)
        {
            return _apiClient.Put<UpdateRuleCommand, CommandCorrelationId>("Rules", command);
        }

        public Task<CommandCorrelationId> SetRuleState(SetRuleStateCommand command)
        {
            return _apiClient.Patch<SetRuleStateCommand, CommandCorrelationId>("Rules/state", command);
        }

        public Task<CommandCorrelationId> DeleteRule(DeleteRuleCommand command)
        {
            return _apiClient.Delete<DeleteRuleCommand, CommandCorrelationId>("Rules", command);
        }
    }
}