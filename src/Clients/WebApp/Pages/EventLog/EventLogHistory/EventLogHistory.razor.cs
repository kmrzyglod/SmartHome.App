using System;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.Blazor;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Components;
using SmartHome.Application.Shared.Events.App;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.Models;
using SmartHome.Application.Shared.Queries.General.GetEvents;
using SmartHome.Clients.WebApp.Helpers;
using SmartHome.Clients.WebApp.Services.EventLog;
using SmartHome.Clients.WebApp.Services.Logger;
using SmartHome.Clients.WebApp.Services.Shared.NotificationsHub;
using SmartHome.Clients.WebApp.Shared.Components.DateRangePicker;

namespace SmartHome.Clients.WebApp.Pages.EventLog.EventLogHistory
{
    public class EventLogHistoryModel : ComponentBase, IDisposable
    {
        private bool _isNewEventComing;
        private readonly Task _refreshDataWhenNewEventComesTask;
        public bool AutoUpdateCheckBox;
        protected DateRangePickerBase DateRangePicker = null!;
        protected DateTime DefaultFromDateTime;
        protected DateTime DefaultToDateTime;

        public EventLogHistoryModel()
        {
            _refreshDataWhenNewEventComesTask = new Task(async () =>
            {
                while (true)
                {
                    if (_isNewEventComing)
                    {
                        await UpdateData();
                        _isNewEventComing = false;
                    }

                    await Task.Delay(500);
                }
            });
        }

        [Inject] protected IEventLogService EventLogService { get; set; } = null!;

        [Inject] protected INotificationsHub NotificationsHub { get; set; } = null!;

        protected PaginationResult<EventVm>? Events { get; set; }
        [Inject] protected IDateTimeProvider DateTimeProvider { get; set; } = null!;
        protected DxDataGrid<EventVm> DataGrid { get; set; } = null!;
        private string NotificationHubSubscriptionId { get; } = Guid.NewGuid().ToString();

        public void Dispose()
        {
            NotificationsHub.Unsubscribe(NotificationHubSubscriptionId);
            _refreshDataWhenNewEventComesTask.Dispose();
        }

        public async Task UpdateData()
        {
            await DataGrid.Refresh();
        }

        protected async Task OnDatesRangeChanged(DateChangedEventArgs eventArgs)
        {
            await UpdateData();
        }

        protected override Task OnInitializedAsync()
        {
            NotificationsHub.Subscribe(nameof(SavedInEventStoreEvent), NotificationHubSubscriptionId, evt =>
            {
                if (!AutoUpdateCheckBox)
                {
                    return Task.CompletedTask;
                }

                _isNewEventComing = true;
                return Task.CompletedTask;
            });

            return Task.CompletedTask;
        }

        protected override void OnInitialized()
        {
            DefaultFromDateTime = DateTimeProvider.GetUtcNow().Date.AddDays(-2);
            DefaultToDateTime = DateTimeProvider.GetUtcNow().Date.AddDays(1);
            _refreshDataWhenNewEventComesTask.Start();
           
        }

        protected async Task<LoadResult> LoadEvents(DataSourceLoadOptionsBase options,
            CancellationToken cancellationToken)
        {
            try
            {
                var query = options.ToApiQuery<GetEventsQuery>();
                query.From = DateRangePicker.FromDate;
                query.To = DateRangePicker.ToDate;
                var result = await EventLogService.GetEvents(query, !AutoUpdateCheckBox);
                Events = result;
                return result.ToDxLoadResult();
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Failed to load events data");
            }

            return new LoadResult();
        }
    }
}