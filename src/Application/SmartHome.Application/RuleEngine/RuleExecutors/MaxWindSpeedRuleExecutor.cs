using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.RuleEngine.OperatorParsers;
using SmartHome.Application.Shared.RulesEngine.Models;

namespace SmartHome.Application.RuleEngine.RuleExecutors
{
    public class MaxWindSpeedRuleExecutor : IRuleExecutor
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IOperatorParser<double> _operatorParser;

        public MaxWindSpeedRuleExecutor(IApplicationDbContext dbContext, IOperatorParser<double> operatorParser)
        {
            _dbContext = dbContext;
            _operatorParser = operatorParser;
        }

        public RuleId RuleId => RuleId.MaxWindSpeed;

        public async Task<bool> Execute(RuleNode rule, CancellationToken cancellationToken)
        {
            double averageMaxWindSpeed = await _dbContext.WeatherStationWindParameters
                .OrderByDescending(x => x.MeasurementStartTime)
                .Select(x => x.MaxWindSpeed)
                .Take(3)
                .AverageAsync(cancellationToken);

            return _operatorParser.CheckCondition(rule, averageMaxWindSpeed);
        }
    }
}