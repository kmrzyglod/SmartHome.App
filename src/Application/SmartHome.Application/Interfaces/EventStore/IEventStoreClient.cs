using System.Threading;
using System.Threading.Tasks;
using SmartHome.Application.Shared.Interfaces.Event;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Interfaces.EventStore
{
    public interface IEventStoreClient
    {
        Task<EventModel> SaveEventAsync(IEvent @event);

        Task<PaginationResult<EventModel>> FindEventsByCriteriaAsync(
            IEventFilteringCriteria eventFilteringCriteria, CancellationToken cancellationToken = default);
    }
}