using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SmartHome.Application.Shared.Events;
using SmartHome.Application.Shared.Events.Devices.WeatherStation.Telemetry;
using SmartHome.Application.Shared.Models;
using SmartHome.Application.Shared.Queries.General.GetEvents;
using SmartHome.Clients.WebApp.Helpers;
using SmartHome.Clients.WebApp.Services.EventLog;
using SmartHome.Clients.WebApp.Services.Logger;
using SmartHome.Clients.WebApp.Services.Shared.NotificationsHub;

namespace SmartHome.Clients.WebApp.Pages.EventLog.EventLogHistory
{
    public class EventLogHistoryModel: ComponentBase, IDisposable
    {
        [Inject] protected IEventLogService EventLogService { get; set; } = null!;
        [Inject] protected INotificationsHub NotificationsHub { get; set; } = null!;
        //protected LoadResult? EventsDxFormatted { get; set; }
        protected PaginationResult<EventVm>? Events { get; set; }

        protected override async Task OnInitializedAsync()
        {
            NotificationsHub.Subscribe<CommandResultEvent>(nameof(EventLogHistoryModel), e  =>
            {
                Console.WriteLine($"EventLogHistoryModel: {e.CommandName}");
            });
        }

        protected async Task<LoadResult> LoadEvents(DataSourceLoadOptionsBase options,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await EventLogService.GetEvents(options.ToApiQuery<GetEventsQuery>());
                Events = result;
                return result.ToDxLoadResult();
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Failed to load events data");
            }

            return new LoadResult();
        }

        public void Dispose()
        {
            NotificationsHub.Unsubscribe(nameof(EventLogHistoryModel));
        }
    }
}
