using System.Threading.Tasks;
using SmartHome.Application.Shared.Models;
using SmartHome.Application.Shared.Queries.General.GetEvents;

namespace SmartHome.Clients.WebApp.Services.EventLog
{
    public interface IEventLogService
    {
        Task<PaginationResult<EventVm>> GetEvents(GetEventsQuery query, bool withCache = true);
    }
}