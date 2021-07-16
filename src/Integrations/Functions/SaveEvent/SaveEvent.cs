using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using SmartHome.Application.Interfaces.EventStore;
using SmartHome.Infrastructure.Attributes;
using SmartHome.Infrastructure.EventBusMessageDeserializer;
using SmartHome.Infrastructure.NotificationService;

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
        [SignalROutput(HubName = "notifications", ConnectionStringSetting = "AzureSignalRConnectionString")]
        public async Task<SignalRMessage> Run([EventGridTrigger] EventGridEvent eventGridEvent, FunctionContext context)
        {
            var @event = await _eventGridMessageDeserializer.DeserializeAsync(eventGridEvent);
            var eventModel = await _eventStoreClient.SaveEventAsync(@event);
            return new SignalRMessage
            {
                Target = eventModel.GetType()
                    .Name,
                Arguments = new object[] {eventModel}
            };
        }
    }
}