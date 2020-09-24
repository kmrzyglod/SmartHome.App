using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Interfaces.EventStore;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Interfaces.Event;
using SmartHome.Application.Shared.Models;
using SmartHome.Infrastructure.Configuration;
using SmartHome.Infrastructure.Extensions;

namespace SmartHome.Infrastructure.EventStore
{
    public class EventStoreClient : IEventStoreClient
    {
        private readonly IConfigProvider _configProvider;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMongoDatabase _eventStoreDatabase;

        public EventStoreClient(IMongoDatabase eventStoreDatabase, IConfigProvider configProvider,
            IDateTimeProvider dateTimeProvider)
        {
            _eventStoreDatabase = eventStoreDatabase;
            _configProvider = configProvider;
            _dateTimeProvider = dateTimeProvider;
            MongoClassMap.RegisterClassMap();
        }

        public Task SaveEventAsync(IEvent @event)
        {
            var collection = _eventStoreDatabase.GetCollection<EventModel>(_configProvider.EventStoreContainer);
            return collection.InsertOneAsync(new EventModel
            {
                EventName = @event.GetType().Name,
                Timestamp = _dateTimeProvider.GetUtcNow(),
                EventType = GetEventType(@event),
                EventData = @event,
                Source =  @event.Source
            });
        }

        public async Task<PaginationResult<EventModel>> FindEventsByCriteriaAsync(
            IEventFilteringCriteria eventFilteringCriteria, CancellationToken cancellationToken = default)
        {
            var collection = _eventStoreDatabase.GetCollection<EventModel>(_configProvider.EventStoreContainer);
            var query = collection.AsQueryable();
            query = AddFilters(eventFilteringCriteria, query);
            int count = await query.CountAsync(cancellationToken);
            var result = await query
                .OrderByDescending(x => x.Id)
                .Skip((eventFilteringCriteria.PageNumber - 1) * eventFilteringCriteria.PageSize)
                .Take(eventFilteringCriteria.PageSize)
                .ToListAsync(cancellationToken);

            return new PaginationResult<EventModel>
            {
                PageNumber = eventFilteringCriteria.PageNumber,
                PageSize = eventFilteringCriteria.PageSize,
                ResultTotalCount = count,
                Result = result
            };
        }

        private static EventType GetEventType(IEvent @event)
        {
            if(@event.GetType().GetInterfaces().Contains(typeof(ICommandResultEvent)))
            {
                return EventType.CommandResult;
            }

            if (@event.GetType().GetInterfaces().Contains(typeof(IErrorEvent)))
            {
                return EventType.Error;
            }

            return EventType.General;

        }

        private IMongoQueryable<EventModel> AddFilters(
            IEventFilteringCriteria eventFilteringCriteria, IMongoQueryable<EventModel> query)
        {
            return query
                .AddFilter(!string.IsNullOrEmpty(eventFilteringCriteria.Source),
                    q => q.Where(x => x.Source == eventFilteringCriteria.Source))
                .AddFilter(!string.IsNullOrEmpty(eventFilteringCriteria.EventName),
                    q => q.Where(x => x.EventName == eventFilteringCriteria.EventName))
                .AddFilter(eventFilteringCriteria.EventType != null,
                    q => q.Where(x => x.EventType == eventFilteringCriteria.EventType))
                .AddFilter(eventFilteringCriteria.From != null,
                    q => q.Where(x => x.Timestamp >= eventFilteringCriteria.From))
                .AddFilter(eventFilteringCriteria.To != null,
                    q => q.Where(x => x.Timestamp <= eventFilteringCriteria.To));
        }
    }
}