using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.RuleEngine.OperatorParsers;
using SmartHome.Application.Shared.RulesEngine.Models;

namespace SmartHome.Application.RuleEngine.RuleExecutors
{
    public class GreenhouseTemperatureRuleExecutor : IRuleExecutor
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IOperatorParser<double> _operatorParser;

        public GreenhouseTemperatureRuleExecutor(IApplicationDbContext dbContext, IOperatorParser<double> operatorParser)
        {
            _dbContext = dbContext;
            _operatorParser = operatorParser;
        }

        public RuleId RuleId => RuleId.Temperature;

        public async Task<bool> Execute(RuleNode rule, CancellationToken cancellationToken)
        {
            double currentTemperatureInGreenhouse = await _dbContext.GreenhouseAirParameters
                .OrderByDescending(x => x.Timestamp)
                .Select(x => x.Temperature)
                .FirstOrDefaultAsync(cancellationToken);

            return _operatorParser.CheckCondition(rule, currentTemperatureInGreenhouse);
        }
    }
}