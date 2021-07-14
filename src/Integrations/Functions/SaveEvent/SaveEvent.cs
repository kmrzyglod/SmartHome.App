using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.Functions.Worker;
using SmartHome.Application.Interfaces.EventStore;
using SmartHome.Infrastructure.Attributes;
using SmartHome.Infrastructure.EventBusMessageDeserializer;

namespace SmartHome.Integrations.Functions.SaveEvent
{
    [HandleError]
    public class SaveEvent
    {
        private readonly IEventGridMessageDeserializer _eventGridMessageDeserializer;
        private readonly IEventStoreClient _eventStoreClient;

        public SaveEvent(IEventGridMessageDeserializer eventGridMessageDeserializer, IEventStoreClient eventStoreClient)
        {
            _eventGridMessageDeserializer = eventGridMessageDeserializer;
            _eventStoreClient = eventStoreClient;
        }

        [Function("SaveEvent")]
        public async Task Run([EventGridTrigger] EventGridEvent eventGridEvent, FunctionContext context)
        {
            var @event = await _eventGridMessageDeserializer.DeserializeAsync(eventGridEvent);
            await _eventStoreClient.SaveEventAsync(@event);
        }
    }
}