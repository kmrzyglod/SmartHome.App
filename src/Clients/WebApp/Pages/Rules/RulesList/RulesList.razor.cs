using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Radzen;
using SmartHome.Application.Shared.Commands.Rules.DeleteRule;
using SmartHome.Application.Shared.Commands.Rules.SetRuleState;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.Queries.Rules.GetRulesList;
using SmartHome.Clients.WebApp.Services.Rules;
using SmartHome.Clients.WebApp.Services.Shared.CommandsExecutor;
using SmartHome.Clients.WebApp.Shared.Components;

namespace SmartHome.Clients.WebApp.Pages.Rules.RulesList
{
    public class RulesListComponent : ComponentWithNotificationHub
    {
        private readonly ConcurrentDictionary<string, bool> _pendingStates = new();
        [Inject] protected IDateTimeProvider DateTimeProvider { get; set; } = null!;

        [Inject] protected ICommandsExecutor CommandsExecutor { get; set; } = null!;

        [Inject] protected IRulesService RulesService { get; set; } = null!;

        [Inject] protected NavigationManager NavManager { get; set; } = null!;

        [Inject] protected NotificationService ToastrNotificationService { get; set; } = null!;

        [Inject] protected DialogService DialogService { get; set; } = null!;

        protected IEnumerable<RulesListEntryVm> Data { get; set; }
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
            await UpdateData();
        }

        private async Task UpdateData()
        {
            SetPending("table", true);
            var result = await RulesService.GetRulesList(new GetRulesListQuery
            {
                PageNumber = PageNumber,
                PageSize = PageSize
            }, false);

            Count = result.ResultTotalCount;
            Data = result.Result;
            SetPending("table", false);
        }

        protected Task OnRuleChangeState(long id, bool state)
        {
            SetPending(id.ToString(), true);
            return CommandsExecutor.ExecuteCommand(command => RulesService.SetRuleState(command),
                new SetRuleStateCommand
                {
                    Id = id,
                    IsActive = state
                }, 30, result =>
                {
                    if (result.Status != StatusCode.Success)
                    {
                        var rule = Data.First(x => x.Id == id);
                        rule.IsActive = !rule.IsActive;
                    }

                    SetPending(id.ToString(), false);
                    StateHasChanged();
                }, hideNotifications: true);
        }

        protected async Task OnDeleteClick(long id)
        {
            SetPending($"{id}_deleting", true);
            bool deleteConfirmed = await DialogService.Confirm("Are you sure?", "Delete rule ",
                new ConfirmOptions {OkButtonText = "Yes", CancelButtonText = "No"}) ?? false;

            if (!deleteConfirmed)
            {
                SetPending($"{id}_deleting", false);
                return;
            }

            await CommandsExecutor.ExecuteCommand(command => RulesService.DeleteRule(command), new DeleteRuleCommand
            {
                Id = id
            }, 30, result =>
            {
                if (result.Status == StatusCode.Success)
                {
                    var rules = Data.Where(x => x.Id == id);
                    Data = Data.Except(rules).ToList();
                }

                SetPending($"{id.ToString()}_deleting", false);
                StateHasChanged();
            }, hideNotifications: true);
        }

        protected async Task LoadData(LoadDataArgs args)
        {
            PageNumber = (int) Math.Ceiling((double) ((args.Skip ?? 1) + 1) / PageSize);
            await UpdateData();
        }
    }
}