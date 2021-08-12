using SmartHome.Application.Shared.RulesEngine.Models;

namespace SmartHome.Application.RuleEngine.OperatorParsers
{
    public interface IOperatorParser<in T>
    {
        bool CheckCondition(RuleNode rule, T value);
    }
}