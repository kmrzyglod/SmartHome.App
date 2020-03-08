using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SmartHome.Application.Shared.Models;
using SmartHome.Application.Shared.Queries.GetEvents;
using SmartHome.Clients.WebApp.Services.EventLog;
using SmartHome.Clients.WebApp.Services.Logger;

namespace SmartHome.Clients.WebApp.Pages.EventLog.EventLogHistory
{
    public class EventLogHistoryModel: ComponentBase
    {
        [Inject] protected IEventLogService? EventLogService { get; set; }
        public PaginationResult<EventVm>? Events { get; set; }
        public IEnumerable<EventVm>? EventsList { get; set; } = new List<EventVm>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Events = await EventLogService.GetEvents(new GetEventsQuery());
                EventsList = Events.Result;
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Failed to load weather data");
            }
        }
    }


}
