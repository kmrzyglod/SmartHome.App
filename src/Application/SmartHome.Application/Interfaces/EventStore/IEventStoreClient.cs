using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHome.Application.Interfaces.Event;
using SmartHome.Application.Models;

namespace SmartHome.Application.Interfaces.EventStore
{
    public interface IEventStoreClient
    {
        Task SaveEventAsync(IEvent @event);
        IEnumerable<EventModel> FindEventsByCriteria(IEventFilteringCriteria eventFilteringCriteria);
    }
}
