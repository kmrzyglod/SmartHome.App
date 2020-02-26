using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using SmartHome.Application.Enums;
using SmartHome.Application.Interfaces.DateTime;
using SmartHome.Application.Interfaces.Event;
using SmartHome.Application.Interfaces.EventStore;
using SmartHome.Application.Models;
using SmartHome.Infrastructure.Configuration;

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
        }

        public Task SaveEventAsync(IEvent @event)
        {
            var collection = _eventStoreDatabase.GetCollection<EventModel>(_configProvider.EventStoreContainer);
            return collection.InsertOneAsync(new EventModel
            {
                EventName = @event.GetType().Name,
                Timestamp = _dateTimeProvider.GetUtcNow(),
                EventType = GetEventType(@event),
                EventData = @event
            });
        }

        public IEnumerable<EventModel> FindEventsByCriteria(IEventFilteringCriteria eventFilteringCriteria)
        {
            throw new NotImplementedException();
        }

        private static EventType GetEventType(IEvent @event)
        {
            return @event.GetType() switch
            {
                ICommandResultEvent _ => EventType.CommandResult,
                IErrorEvent _ => EventType.Error,
                _ => EventType.General
            };
        }
    }
}