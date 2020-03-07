using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartHome.Application.Shared.Interfaces.Event;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Interfaces.EventStore
{
    public interface IEventStoreClient
    {
        Task SaveEventAsync(IEvent @event);

        Task<PaginationResult<EventModel>> FindEventsByCriteriaAsync(
            IEventFilteringCriteria eventFilteringCriteria, int pageNumber, int pageSize = 10,
            CancellationToken cancellationToken = default);
    }
}
