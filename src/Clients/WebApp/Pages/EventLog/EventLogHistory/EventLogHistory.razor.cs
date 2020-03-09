using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Components;
using SmartHome.Application.Shared.Models;
using SmartHome.Application.Shared.Queries.GetEvents;
using SmartHome.Clients.WebApp.Helpers;
using SmartHome.Clients.WebApp.Services.EventLog;
using SmartHome.Clients.WebApp.Services.Logger;

namespace SmartHome.Clients.WebApp.Pages.EventLog.EventLogHistory
{
    public class EventLogHistoryModel: ComponentBase
    {
        [Inject] protected IEventLogService? EventLogService { get; set; }
        //protected LoadResult? EventsDxFormatted { get; set; }
        protected PaginationResult<EventVm>? Events { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //try
            //{
            //    Events = await EventLogService.GetEvents(new GetEventsQuery());
            //    //EventsDxFormatted = Events.ToDxLoadResult();
            //}
            //catch (Exception ex)
            //{
            //    Logger.LogWarning(ex, "Failed to load weather data");
            //}
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
    }


}
