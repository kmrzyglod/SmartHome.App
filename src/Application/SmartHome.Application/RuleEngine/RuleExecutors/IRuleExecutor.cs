using System.Threading;
using System.Threading.Tasks;
using SmartHome.Application.Shared.RulesEngine.Models;

namespace SmartHome.Application.RuleEngine.RuleExecutors
{
    public interface IRuleExecutor
    {
        public RuleId RuleId { get;}
        Task<bool> Execute(RuleNode rule, CancellationToken cancellationToken);
    }
}