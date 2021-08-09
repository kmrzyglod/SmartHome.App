using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Radzen;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Clients.WebApp.Services.Shared.CommandsExecutor;
using SmartHome.Clients.WebApp.Shared.Components;

namespace SmartHome.Clients.WebApp.Pages.Rules.RulesList
{
    public class RulesListComponent : ComponentWithNotificationHub
    {
        [Inject] protected IDateTimeProvider DateTimeProvider { get; set; } = null!;

        [Inject] protected ICommandsExecutor CommandsExecutor { get; set; } = null!;

        [Inject] protected NotificationService ToastrNotificationService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}