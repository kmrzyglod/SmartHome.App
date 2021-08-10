using System;
using System.Collections.Generic;
using System.Text;
using SmartHome.Application.Shared.RulesEngine.Models;

namespace SmartHome.Application.Shared.RulesEngine.Helpers
{
    public static class RuleOutputActionHelpers
    {
        public static IEnumerable<RuleOutputAction> AllRuleOutputActions { get; } = new List<RuleOutputAction>
        {
            new RuleOutputAction
            {
                Id = RuleOutputActionId.CloseWindows,
                Name = "Close windows",
            },
            new RuleOutputAction
            {
                Id = RuleOutputActionId.OpenWindows,
                Name = "Open windows"
            },
            new RuleOutputAction
            {
                Id = RuleOutputActionId.SendEmail,
                Name = "Send email"
            },
            new RuleOutputAction
            {
                Id = RuleOutputActionId.Irrigate,
                Name = "Irrigate"
            }
        };
    }
}
