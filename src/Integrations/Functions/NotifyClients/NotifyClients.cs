using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.Http;
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

        [FunctionName("NotificationsHubNegotiate")]
        public static SignalRConnectionInfo GetSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "notifications")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }
        
        [FunctionName("NotifyClients")]
        public async Task Run([EventGridTrigger] EventGridEvent eventGridEvent,
            [SignalR(HubName = "notifications")] IAsyncCollector<SignalRMessage> signalRMessages, ILogger log)
        {
            var @event = await _eventGridMessageDeserializer.DeserializeAsync(eventGridEvent);
            await signalRMessages.AddAsync(new SignalRMessage
            {
                Target = @event.GetType().Name,
                Arguments = new object[] {@event}
            });
        }
    }
}