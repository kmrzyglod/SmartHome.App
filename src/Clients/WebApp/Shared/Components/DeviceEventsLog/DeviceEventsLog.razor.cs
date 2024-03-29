﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Events.App;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.Queries.General.GetEvents;
using SmartHome.Clients.WebApp.Services.EventLog;

namespace SmartHome.Clients.WebApp.Shared.Components.DeviceEventsLog
{
    public class DeviceEventsLogComponent : ComponentWithNotificationHub
    {
        protected IEnumerable<EventVm> Data { get; set; } = Enumerable.Empty<EventVm>();
        protected int Count { get; set; }
        protected bool IsLoading { get; set; }
        [Parameter] public string DeviceId { get; set; } = string.Empty;
        [Parameter] public DateTime FromDate { get; set; }
        [Parameter] public int PageSize { get; set; } = 5;
        [Inject] protected IEventLogService EventLogService { get; set; } = null!;
        [Inject] protected IDateTimeProvider DateTimeProvider { get; set; } = null!;
        protected RadzenDataGrid<EventVm> Grid = null!;

        private int PageNumber { get; set; } = 1;
        private IEnumerable<FilterDescriptor> Filters { get; set; } = Enumerable.Empty<FilterDescriptor>();

        protected override async Task OnInitializedAsync()
        {
            if (FromDate == default)
            {
                FromDate = DateTimeProvider.GetUtcNow().AddDays(-1);
            }

            SubscribeToEventNotifications();
            await UpdateData();
            await base.OnInitializedAsync();
        }

        private void SubscribeToEventNotifications()
        {
            NotificationsHub.Subscribe<SavedInEventStoreEvent>(NotificationHubSubscriptionId,
                evt =>
                {
                    
                    return evt.Source != DeviceId ? Task.CompletedTask : Grid.Reload();
                });
        }

        private async Task UpdateData()
        {
            IsLoading = true;
            var query = new GetEventsQuery
            {
                From = FromDate,
                Source = DeviceId,
                PageSize = PageSize,
                PageNumber = PageNumber
            };

            if (Filters.Any(x => x.Property == nameof(EventVm.EventName)))
            {
                query.EventName = Filters.FirstOrDefault(x => x.Property == nameof(EventVm.EventName))
                    ?.FilterValue
                    .ToString();
            }

            if (Filters.Any(x => x.Property == nameof(EventVm.EventType)))
            {
                query.EventType = Enum.Parse<EventType>(Filters
                    .FirstOrDefault(x => x.Property == nameof(EventVm.EventType))
                    ?.FilterValue
                    .ToString() ?? string.Empty, true);
            }

            var result = await EventLogService.GetEvents(query, false);

            Data = result.Result;
            Count = result.ResultTotalCount;
            IsLoading = false;
        }

        protected async Task LoadData(LoadDataArgs args)
        {
            Filters = args.Filters;
            PageNumber = (int) Math.Ceiling((double) ((args.Skip ?? 1) + 1) / PageSize);
            await UpdateData();
        }
    }
}