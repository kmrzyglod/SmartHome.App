using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Helpers;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Helpers.JsonHelpers;
using SmartHome.Application.Shared.RulesEngine.Models;

namespace SmartHome.Application.RuleEngine.RuleExecutors
{
    public class IsRainingRuleExecutor : IRuleExecutor
    {
        private const double IS_RAINING_PRECIPITATION_SUM = 0.3;
        private readonly IApplicationDbContext _dbContext;

        public IsRainingRuleExecutor(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public RuleId RuleId => RuleId.IsRaining;

        public async Task<bool> Execute(RuleNode rule, CancellationToken cancellationToken)
        {
            double precipitationSumLastHalfHour = await _dbContext.WeatherStationPrecipitation
                .OrderByDescending(x => x.MeasurementStartTime)
                .Select(x => x.Rain)
                .Take(6)
                .SumAsync(cancellationToken);

            bool ruleValue = JsonSerializerHelpers.DeserializeFromObject<bool>(rule.value!);

            return ruleValue
                ? precipitationSumLastHalfHour > IS_RAINING_PRECIPITATION_SUM
                : precipitationSumLastHalfHour <= IS_RAINING_PRECIPITATION_SUM;
        }
    }
}