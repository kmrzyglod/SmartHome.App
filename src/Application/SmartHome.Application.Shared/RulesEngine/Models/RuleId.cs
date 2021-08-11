using System.Runtime.Serialization;

namespace SmartHome.Application.Shared.RulesEngine.Models
{
    public enum RuleId
    {
        [EnumMember(Value = "temperature")] Temperature,
        [EnumMember(Value = "max_wind_speed")] MaxWindSpeed,
        [EnumMember(Value = "is_raining")] IsRaining,
        [EnumMember(Value = "cron_expression")]
        CronExpression
    }
}