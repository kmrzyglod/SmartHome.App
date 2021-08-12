using System.Runtime.Serialization;

namespace SmartHome.Application.Shared.RulesEngine.Models
{
    public enum RuleCondition
    {
        [EnumMember(Value = "AND")]
        And,
        [EnumMember(Value = "OR")]
        Or
    }
}