using System.Threading;
using System.Threading.Tasks;
using SmartHome.Application.Shared.RulesEngine.Models;

namespace SmartHome.Application.RuleEngine
{
    public interface IRuleEngine
    {
        Task<bool> Execute(long ruleId, RuleNode rule, RuleOutputAction outputAction, CancellationToken cancellationToken);
    }
}