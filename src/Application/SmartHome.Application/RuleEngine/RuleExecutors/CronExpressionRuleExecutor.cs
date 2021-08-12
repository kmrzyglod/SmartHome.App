using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Cronos;
using SmartHome.Application.Helpers;
using SmartHome.Application.Shared.Helpers.JsonHelpers;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.RulesEngine.Models;

namespace SmartHome.Application.RuleEngine.RuleExecutors
{
    public class CronExpressionRuleExecutor : IRuleExecutor
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public CronExpressionRuleExecutor(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public RuleId RuleId => RuleId.CronExpression;

        public Task<bool> Execute(RuleNode rule, CancellationToken cancellationToken)
        {
            var currentUtc = _dateTimeProvider.GetUtcNow();
            string? ruleValue = JsonSerializerHelpers.DeserializeFromObject<string>(rule.value!);

            CronExpression expression = CronExpression.Parse(ruleValue);
            var nextUtc = expression.GetNextOccurrence(currentUtc);

            if (nextUtc == null)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult((nextUtc.Value - currentUtc).TotalMinutes <= 5);
        }
    }
}