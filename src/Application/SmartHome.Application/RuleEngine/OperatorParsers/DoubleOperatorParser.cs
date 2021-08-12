using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using SmartHome.Application.Extensions;
using SmartHome.Application.Helpers;
using SmartHome.Application.Shared.Helpers.JsonHelpers;
using SmartHome.Application.Shared.RulesEngine.Models;

namespace SmartHome.Application.RuleEngine.OperatorParsers
{
    public class DoubleOperatorParser : IOperatorParser<double>
    {
        private readonly IDictionary<RuleOperator, Func<object, double, bool>> _operatorMappings =
            new Dictionary<RuleOperator, Func<object, double, bool>>
            {
                {RuleOperator.Equal, (ruleValue, value) => Math.Abs((double) ruleValue - value) < 0.1},
                {RuleOperator.NotEqual, (ruleValue, value) => Math.Abs(value - (double) ruleValue) > 0.1},
                {
                    RuleOperator.Between, (ruleValue, value) =>
                    {
                        var ruleValues = (IEnumerable<double>) ruleValue;
                        return value >= ruleValues.First() && value <= ruleValues.Last();
                    }
                },
                {
                    RuleOperator.NotBetween, (ruleValue, value) =>
                    {
                        var ruleValues = (IEnumerable<double>) ruleValue;
                        return value < ruleValues.First() && value > ruleValues.Last();
                    }
                },
                {RuleOperator.Greater, (ruleValue, value) => value > (double) ruleValue},
                {RuleOperator.GreaterOrEqual, (ruleValue, value) => value >= (double) ruleValue},
                {RuleOperator.Less, (ruleValue, value) => value < (double) ruleValue},
                {RuleOperator.LessOrEqual, (ruleValue, value) => value <= (double) ruleValue}
            };

        public RuleType RuleType => RuleType.Double;

        public bool CheckCondition(RuleNode rule, double value)
        {
            var ruleOperator = rule.@operator!.ParseEnum<RuleOperator>();
            object? ruleValue = ruleOperator == RuleOperator.Between || ruleOperator == RuleOperator.NotBetween
                ? JsonSerializerHelpers.DeserializeFromObject<IEnumerable<double>>(rule.value!)
                : (object) JsonSerializerHelpers.DeserializeFromObject<double>(rule.value!);

            return _operatorMappings[ruleOperator](ruleValue, value);
        }
    }
}