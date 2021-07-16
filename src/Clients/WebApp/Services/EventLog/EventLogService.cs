using System.Threading.Tasks;
using SmartHome.Application.Shared.Models;
using SmartHome.Application.Shared.Queries.General.GetEvents;
using SmartHome.Clients.WebApp.Services.Shared.ApiClient;

namespace SmartHome.Clients.WebApp.Services.EventLog
{
    public class EventLogService : IEventLogService
    {
        private readonly IApiClient _apiClient;

        public EventLogService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public Task<PaginationResult<EventVm>> GetEvents(GetEventsQuery query, bool withCache = true)
        {
            return _apiClient.Get<GetEventsQuery, PaginationResult<EventVm>>("EventLog", query,  withCache ? null : _apiClient.NoCacheHeader);
        }
    }
}
