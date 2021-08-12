using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using SmartHome.Application.Shared.Commands.Devices.GreenhouseController.Irrigation;
using SmartHome.Application.Shared.Commands.Devices.WindowsController.CloseWindow;
using SmartHome.Application.Shared.Commands.Devices.WindowsController.OpenWindow;
using SmartHome.Application.Shared.Commands.Rules.AddRule;
using SmartHome.Application.Shared.Commands.Rules.UpdateRule;
using SmartHome.Application.Shared.Commands.SendEmail;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Helpers.JsonHelpers;
using SmartHome.Application.Shared.Interfaces.Command;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.Queries.Rules.GetRuleDetails;
using SmartHome.Application.Shared.RulesEngine.Helpers;
using SmartHome.Application.Shared.RulesEngine.Models;
using SmartHome.Clients.WebApp.Helpers;
using SmartHome.Clients.WebApp.Services.Rules;
using SmartHome.Clients.WebApp.Services.Shared.CommandsExecutor;
using SmartHome.Clients.WebApp.Shared.Components;

namespace SmartHome.Clients.WebApp.Pages.Rules.ManageRule
{
    public class ManageRuleComponent : ComponentWithNotificationHub
    {
        protected const string GREENHOUSE_CONTROLLER_DEVICE_ID = "devices/esp32-greenhouse";
        protected const string WINDOWS_CONTROLLER_DEVICE_ID = "devices/esp32-windows-controller";
        protected int EnabledStep = 1;

        [Inject] protected IDateTimeProvider DateTimeProvider { get; set; } = null!;
        [Inject] protected ICommandsExecutor CommandsExecutor { get; set; } = null!;
        [Inject] protected NotificationService ToastrNotificationService { get; set; } = null!;
        [Inject] protected IJSRuntime JsRuntime { get; set; } = null!;
        [Inject] protected IRulesService RulesService { get; set; } = null!;
        [Inject] private NavigationManager NavManager { get; set; } = null!;

        [Parameter] public int? RuleId { get; set; }

        private string SerializedRules { get; set; } = string.Empty;
        protected string RuleName { get; set; } = string.Empty;

        protected IEnumerable<int> OutputActionSelectedWindows { get; set; } = Enumerable.Empty<int>();
        protected SendEmailCommand SendEmailCommand { get; set; } = new();

        protected IrrigateCommand IrrigateCommand { get; set; } = new IrrigateCommand
        {
            TargetDeviceId = GREENHOUSE_CONTROLLER_DEVICE_ID,
        };

        protected bool IsSavingInProgress { get; set; }

        protected RuleOutputAction SelectedOutputAction { get; set; } = new()
        {
            Id = RuleOutputActionId.None
        };

        protected IEnumerable<RuleOutputAction> RuleOutputActions => RuleOutputActionHelpers.AllRuleOutputActions;

        protected override async Task OnInitializedAsync()
        {
            if (RuleId != null)
            {
                await LoadSavedRule(RuleId.Value);
            }

            await base.OnInitializedAsync();
        }

        protected Task SaveRule()
        {
            IsSavingInProgress = true;
            var deserializedBody = JsonSerializer.Deserialize<RuleNode>(SerializedRules, CustomJsonSerializerOptionsProvider.OptionsEnumConverter);
            if (deserializedBody == null)
            {
                IsSavingInProgress = false;
                ToastrNotificationService.Notify(NotificationSeverity.Error, "Error", "Invalid rules model");
                return Task.CompletedTask;
            }

            return RuleId == null
                ? CommandsExecutor.ExecuteCommand(command => RulesService.AddRule(command), new AddRuleCommand
                {
                    Body = deserializedBody,
                    Name = RuleName,
                    OutputAction = SelectedOutputAction
                }, 30, result =>
                {
                    IsSavingInProgress = false;
                    StateHasChanged();
                    if (result.Status != StatusCode.Success)
                    {
                        return;
                    }

                    NavManager.NavigateTo("/rules/list");
                })
                : CommandsExecutor.ExecuteCommand(command => RulesService.UpdateRule(command), new UpdateRuleCommand
                {
                    Id = RuleId.Value,
                    Body = deserializedBody,
                    Name = RuleName,
                    OutputAction = SelectedOutputAction
                }, 30, result =>
                {
                    IsSavingInProgress = false;
                    StateHasChanged();
                    if (result.Status != StatusCode.Success)
                    {
                        return;
                    }

                    NavManager.NavigateTo("/rules/list");
                });
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
            SelectedOutputAction.Commands = SelectedOutputAction.Id switch
            {
                RuleOutputActionId.CloseWindows => OutputActionSelectedWindows.Select(x => new CloseWindowCommand
                {
                    WindowId = (ushort) x, TargetDeviceId = WINDOWS_CONTROLLER_DEVICE_ID
                }).ToList(),
                RuleOutputActionId.OpenWindows => OutputActionSelectedWindows.Select(x => new OpenWindowCommand
                {
                    WindowId = (ushort) x, TargetDeviceId = WINDOWS_CONTROLLER_DEVICE_ID
                }).ToList(),
                RuleOutputActionId.Irrigate => new List<ICommand> {IrrigateCommand},
                RuleOutputActionId.SendEmail => new List<ICommand> {SendEmailCommand},
                _ => SelectedOutputAction.Commands
            };
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
            bool validationResult = await JsRuntime.InvokeAsync<bool>("validateRules", "query-builder");
            if (validationResult)
            {
                EnabledStep = 2;
                return;
            }

            EnabledStep = 1;
        }

        private async Task LoadSavedRule(int ruleId)
        {
            var result = await RulesService.GetRuleDetails(new GetRuleDetailsQuery
            {
                Id = ruleId
            }, withCache: false);
            RuleName = result.Name;
            SerializedRules = JsonSerializer.Serialize(result.Body, CustomJsonSerializerOptionsProvider.OptionsEnumConverter);
            DeserializeOutputAction(result.OutputAction);
        }

        private void DeserializeOutputAction(RuleOutputAction outputAction)
        {
            SelectedOutputAction = outputAction;
            switch (SelectedOutputAction.Id)
            {
                case RuleOutputActionId.CloseWindows:
                    OutputActionSelectedWindows =
                        SelectedOutputAction.Commands.Select(x => (int) JsonSerializerHelpers.DeserializeFromObject<CloseWindowCommand>(x).WindowId).ToList();
                    break;
                case RuleOutputActionId.OpenWindows:
                    OutputActionSelectedWindows =
                        SelectedOutputAction.Commands.Select(x => (int) JsonSerializerHelpers.DeserializeFromObject<OpenWindowCommand>(x).WindowId).ToList();
                    break;
                case RuleOutputActionId.SendEmail:
                    SendEmailCommand =
                        JsonSerializerHelpers.DeserializeFromObject<SendEmailCommand>(SelectedOutputAction.Commands
                            .First());
                    break;
                case RuleOutputActionId.Irrigate:
                    IrrigateCommand =
                        JsonSerializerHelpers.DeserializeFromObject<IrrigateCommand>(SelectedOutputAction.Commands
                            .First());
                    break;
            }
        }
    }
}