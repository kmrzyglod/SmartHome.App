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
                Id = "CloseWindows",
                Name = "Close windows"
            },
            new RuleOutputAction
            {
                Id = "OpenWindows",
                Name = "Open windows"
            },
            new RuleOutputAction
            {
                Id = "SendEmail",
                Name = "Send email"
            },
            new RuleOutputAction
            {
                Id = "Irrigate",
                Name = "Irrigate"
            }
        };
    }
}
