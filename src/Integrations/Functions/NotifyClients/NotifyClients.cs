using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using SmartHome.Infrastructure.EventBusMessageDeserializer;

namespace SmartHome.Integrations.Functions.NotifyClients
{
    public class NotifyClients
    {
        private readonly IEventGridMessageDeserializer _eventGridMessageDeserializer;

        public NotifyClients(IEventGridMessageDeserializer eventGridMessageDeserializer)
        {
            _eventGridMessageDeserializer = eventGridMessageDeserializer;
        }

        [FunctionName("NotifyClients")]
        public async Task Run([EventGridTrigger] EventGridEvent eventGridEvent,
            [SignalR(HubName = "notifications")] IAsyncCollector<SignalRMessage> signalRMessages, ILogger log)
        {
            var @event = await _eventGridMessageDeserializer.DeserializeAsync(eventGridEvent);
            await signalRMessages.AddAsync(new SignalRMessage
            {
                Target = nameof(@event),
                Arguments = new object[] {@event}
            });
        }
    }
}