using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.RulesEngine.Helpers;
using SmartHome.Application.Shared.RulesEngine.Models;
using SmartHome.Clients.WebApp.Services.Shared.CommandsExecutor;
using SmartHome.Clients.WebApp.Shared.Components;

namespace SmartHome.Clients.WebApp.Pages.Rules.ManageRule
{
    public class ManageRuleComponent : ComponentWithNotificationHub
    {
        [Inject] protected IDateTimeProvider DateTimeProvider { get; set; } = null!;

        [Inject] protected ICommandsExecutor CommandsExecutor { get; set; } = null!;

        [Inject] protected NotificationService ToastrNotificationService { get; set; } = null!;
        [Inject] protected IJSRuntime JsRuntime { get; set; } = null!;

        [Parameter] public int? RuleId { get; set; }

        private string SerializedRules { get; set; } = string.Empty;
        protected int EnabledStep = 1;
        protected string RuleName { get; set; } = string.Empty;

        protected IEnumerable<RuleOutputAction> RuleOutputActions => RuleOutputActionHelpers.AllRuleOutputActions;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected void OnRuleNameChanged(string ruleName)
        {
            if (string.IsNullOrEmpty(ruleName))
            {
                EnabledStep = 0;
                return;
            }

            EnabledStep = 1;
        }

        protected void OnRuleOutputActionChange(object ruleOutputActionId)
        {

        }

        protected async Task OnStepChange(int index)
        {
            switch (index)
            {
                case 0:
                    EnabledStep = 1;
                    break;
                case 1:
                    await Task.Delay(5);
                    EnabledStep = 1;
                    await JsRuntime.InvokeVoidAsync("initQueryBuilder", "query-builder", SerializedRules);
                    break;
                case 2:
                    SerializedRules = await JsRuntime.InvokeAsync<string>("getRules", "query-builder");
                    break;
            }

        }

        protected async Task OnRulesValidateClick()
        {
            var validationResult = await JsRuntime.InvokeAsync<bool>("validateRules", "query-builder");
            if (validationResult)
            {
                EnabledStep = 2;
                return;
            }

            EnabledStep = 1;
        }
    }
}