using System.Runtime.Serialization;

namespace SmartHome.Application.Shared.RulesEngine.Models
{
    public enum RuleOperator
    {
        [EnumMember(Value = "equal")] Equal,
        [EnumMember(Value = "not_equal")] NotEqual,
        [EnumMember(Value = "less")] Less,
        [EnumMember(Value = "less_or_equal")] LessOrEqual,
        [EnumMember(Value = "greater")] Greater,
        [EnumMember(Value = "greater_or_equal")]
        GreaterOrEqual,
        [EnumMember(Value = "between")] Between,
        [EnumMember(Value = "not_between")] NotBetween
    }
}