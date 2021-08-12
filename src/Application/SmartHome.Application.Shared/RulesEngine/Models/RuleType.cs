using System.Runtime.Serialization;

namespace SmartHome.Application.Shared.RulesEngine.Models
{
    public enum RuleType
    {
        [EnumMember(Value = "boolean")] Boolean,
        [EnumMember(Value = "double")] Double,
        [EnumMember(Value = "string")] String
    }
}