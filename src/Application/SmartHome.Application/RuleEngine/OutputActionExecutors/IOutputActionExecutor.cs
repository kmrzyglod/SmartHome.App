using System.Threading;
using System.Threading.Tasks;
using SmartHome.Application.Shared.RulesEngine.Models;

namespace SmartHome.Application.RuleEngine.OutputActionExecutors
{
    public interface IOutputActionExecutor
    {
        public RuleOutputActionId OutputActionId { get;}
        Task<bool> Execute(long ruleId, RuleOutputAction outputAction, CancellationToken cancellationToken);
    }
}