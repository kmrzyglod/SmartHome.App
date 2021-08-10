using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using SmartHome.Application.Shared.Commands.Devices.GreenhouseController.Irrigation;
using SmartHome.Application.Shared.Commands.Devices.WindowsController.CloseWindow;
using SmartHome.Application.Shared.Commands.SendEmail;
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

        protected IEnumerable<int> OutputActionSelectedWindows { get; set; } = Enumerable.Empty<int>();
        protected SendEmailCommand SendEmailCommand { get; set; } = new SendEmailCommand();
        protected IrrigateCommand IrrigateCommand { get; set; } = new IrrigateCommand();
        protected RuleOutputAction SelectedOutputAction { get; set; } = new()
        {
            Id = RuleOutputActionId.None
        };

        protected IEnumerable<RuleOutputAction> RuleOutputActions => RuleOutputActionHelpers.AllRuleOutputActions;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected Task SaveRule()
        {
            return Task.CompletedTask;
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
            SelectedOutputAction = RuleOutputActions.First(x => x.Id == (RuleOutputActionId) ruleOutputActionId);
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