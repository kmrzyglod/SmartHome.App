using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SmartHome.Application.Extensions;
using SmartHome.Application.Interfaces.EventBus;
using SmartHome.Application.Logging;
using SmartHome.Application.RuleEngine.OutputActionExecutors;
using SmartHome.Application.RuleEngine.RuleExecutors;
using SmartHome.Application.Shared.Events.App;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.RulesEngine.Models;
using SmartHome.Domain.Entities.Rules;

namespace SmartHome.Application.RuleEngine
{
    public class RuleEngine : IRuleEngine
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEventBus _eventBus;
        private readonly ILogger<LoggingContext> _logger;
        private readonly IEnumerable<IOutputActionExecutor> _outputActionExecutors;

        private readonly IDictionary<RuleCondition, Func<bool, bool, bool>> _ruleConditionMapping =
            new Dictionary<RuleCondition, Func<bool, bool, bool>>
            {
                {
                    RuleCondition.And, (prev, next) => prev && next
                },
                {
                    RuleCondition.Or, (prev, next) => prev || next
                }
            };

        private readonly IEnumerable<IRuleExecutor> _rulesExecutors;

        public RuleEngine(IEnumerable<IRuleExecutor> rulesExecutors,
            IEnumerable<IOutputActionExecutor> outputActionExecutors, IEventBus eventBus,
            ILogger<LoggingContext> logger, IDateTimeProvider dateTimeProvider)
        {
            _rulesExecutors = rulesExecutors;
            _outputActionExecutors = outputActionExecutors;
            _eventBus = eventBus;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<bool> Execute(long ruleId, RuleNode rule, RuleOutputAction outputAction,
            CancellationToken cancellationToken)
        {
            try
            {
                bool ruleCalculationResult = await VisitNode(rule, cancellationToken);
                if (ruleCalculationResult == false)
                {
                    _logger.LogInformation($"[RuleEngine] Rule with id: {ruleId} ignored");
                    return false;
                }

                var ruleOutputActionExecutor = _outputActionExecutors.Single(x => x.OutputActionId == outputAction.Id);
                bool outputActionExecutionResult =
                    await ruleOutputActionExecutor.Execute(ruleId, outputAction, cancellationToken);

                if (outputActionExecutionResult)
                {
                    _logger.LogInformation($"[RuleEngine] Rule with id: {ruleId} executed");
                    await _eventBus.Publish(new RuleExecutionResultEvent
                    {
                        ExecutionStatus = RuleExecutionStatus.Success,
                        RuleId = ruleId,
                        Timestamp = _dateTimeProvider.GetUtcNow()
                    });
                }
                else
                {
                    _logger.LogInformation($"[RuleEngine] Rule with id: {ruleId} ignored");
                }

                return outputActionExecutionResult;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"[RuleEngine] Error during executing rule with id: {ruleId}");
                await _eventBus.Publish(new RuleExecutionResultEvent
                {
                    ExecutionStatus = RuleExecutionStatus.Failed,
                    ErrorMessage = e.Message,
                    RuleId = ruleId,
                    Timestamp = _dateTimeProvider.GetUtcNow()
                });

                return false;
            }
        }

        private async Task<bool> VisitNode(RuleNode rule, CancellationToken cancellationToken)
        {
            //If no leaf go recursive through rules collection 
            if (rule.rules != null && rule.rules.Any() && rule.condition != null)
            {
                var ruleCondition = rule.condition.ParseEnum<RuleCondition>();
                var ruleResults = new List<bool>();
                foreach (var ruleRule in rule.rules)
                {
                    ruleResults.Add(await VisitNode(ruleRule, cancellationToken));
                }
               
                return ruleResults
                    .Aggregate(_ruleConditionMapping[ruleCondition]);
            }

            //If leaf execute condition calculation logic 
            if (rule.id == null)
            {
                throw new Exception("Invalid rule definition. Rule id can't be empty");
            }

            var ruleId = rule.id.ParseEnum<RuleId>();
            var ruleExecutor = _rulesExecutors.Single(x => x.RuleId == ruleId);
            return await ruleExecutor.Execute(rule, cancellationToken);
        }
    }
}