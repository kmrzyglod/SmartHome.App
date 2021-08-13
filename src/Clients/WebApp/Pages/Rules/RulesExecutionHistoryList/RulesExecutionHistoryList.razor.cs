using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Radzen;
using SmartHome.Application.Shared.Events.App;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.Queries.Rules.GetRulesExecutionHistoryList;
using SmartHome.Clients.WebApp.Services.Rules;
using SmartHome.Clients.WebApp.Shared.Components;

namespace SmartHome.Clients.WebApp.Pages.Rules.RulesExecutionHistoryList
{
    public class RulesExecutionHistoryListComponent : ComponentWithNotificationHub
    {
        private readonly ConcurrentDictionary<string, bool> _pendingStates = new();
        [Inject] protected IDateTimeProvider DateTimeProvider { get; set; } = null!;

        [Inject] protected IRulesService RulesService { get; set; } = null!;

        [Inject] protected NavigationManager NavManager { get; set; } = null!;

        [Inject] protected NotificationService ToastrNotificationService { get; set; } = null!;

        protected IEnumerable<RulesExecutionHistoryListVm> Data { get; set; }
        private int PageNumber { get; set; } = 1;
        private int PageSize { get; } = 15;
        protected int Count { get; set; }

        protected bool IsPending(string id)
        {
            return _pendingStates.TryGetValue(id, out bool state) && state;
        }

        protected void SetPending(string id, bool state)
        {
            _pendingStates.AddOrUpdate(id, state, (key, value) => state);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            SubscribeToRuleExecutionNotifications();
            await UpdateData();
        }

        private void SubscribeToRuleExecutionNotifications()
        {
            NotificationsHub.Subscribe<RuleExecutionResultEvent>(NotificationHubSubscriptionId, async () =>
            {
                await UpdateData(false);
                StateHasChanged();
            });
        }
        
        private async Task UpdateData(bool withCache = true)
        {
            SetPending("table", true);
            var result = await RulesService.GetRulesExecutionListHistory(new GetRulesExecutionHistoryListQuery
            {
                PageNumber = PageNumber,
                PageSize = PageSize
            }, withCache);

            Count = result.ResultTotalCount;
            Data = result.Result;
            SetPending("table", false);
        }

        protected async Task LoadData(LoadDataArgs args)
        {
            PageNumber = (int) Math.Ceiling((double) ((args.Skip ?? 1) + 1) / PageSize);
            await UpdateData();
        }
    }
}