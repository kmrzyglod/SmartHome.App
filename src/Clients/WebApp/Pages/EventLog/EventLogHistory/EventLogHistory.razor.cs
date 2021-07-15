using System;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.Blazor;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Components;
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
        public bool AutoUpdateCheckBox;
        protected DateRangePickerBase DateRangePicker = null!;
        protected DateTime DefaultFromDateTime;
        protected DateTime DefaultToDateTime;
        [Inject] protected IEventLogService EventLogService { get; set; } = null!;

        [Inject] protected INotificationsHub NotificationsHub { get; set; } = null!;

        //protected LoadResult? EventsDxFormatted { get; set; }
        protected PaginationResult<EventVm>? Events { get; set; }
        [Inject] protected IDateTimeProvider DateTimeProvider { get; set; } = null!;
        protected DxDataGrid<EventVm> DataGrid { get; set; } = null!;

        public void Dispose()
        {
            //NotificationsHub.Unsubscribe(nameof(EventLogHistoryModel));
        }

        public async Task UpdateData()
        {
            await DataGrid.Refresh();
        }

        protected async Task OnDatesRangeChanged(DateChangedEventArgs eventArgs)
        {
            await UpdateData();
        }

        protected override void OnInitialized()
        {
            DefaultFromDateTime = DateTimeProvider.GetUtcNow().Date.AddDays(-2);
            DefaultToDateTime = DateTimeProvider.GetUtcNow().Date.AddDays(1);
        }

        protected override async Task OnInitializedAsync()
        {
            //NotificationsHub.Subscribe<CommandResultEvent>(nameof(EventLogHistoryModel), e  =>
            //{
            //    Console.WriteLine($"EventLogHistoryModel: {e.CommandName}");
            //});
        }

        protected async Task<LoadResult> LoadEvents(DataSourceLoadOptionsBase options,
            CancellationToken cancellationToken)
        {
            try
            {
                var query = options.ToApiQuery<GetEventsQuery>();
                query.From = DateRangePicker.FromDate;
                query.To = DateRangePicker.ToDate;
                var result = await EventLogService.GetEvents(query);
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