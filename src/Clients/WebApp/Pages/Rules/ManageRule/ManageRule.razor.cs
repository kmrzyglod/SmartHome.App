using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using SmartHome.Application.Shared.Interfaces.DateTime;
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

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected async Task OnStepChange(int index)
        {
            await Task.Delay(5);
            if (index == 1)
            {
                await JsRuntime.InvokeVoidAsync("initQueryBuilder", "query-builder");
            }
        }
    }
}