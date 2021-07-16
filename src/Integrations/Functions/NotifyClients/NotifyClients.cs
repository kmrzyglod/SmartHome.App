using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using SmartHome.Infrastructure.EventBusMessageDeserializer;
using SmartHome.Infrastructure.NotificationService;

namespace SmartHome.Integrations.Functions.NotifyClients
{
    public class NotifyClients
    {
        private readonly IEventGridMessageDeserializer _eventGridMessageDeserializer;

        public NotifyClients(IEventGridMessageDeserializer eventGridMessageDeserializer)
        {
            _eventGridMessageDeserializer = eventGridMessageDeserializer;
        }

        [Function("NotificationsHubNegotiate")]
        public static async Task<HttpResponseData> GetSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Function, "post")]
            HttpRequestData req,
            [SignalRConnectionInfoInput(HubName = "notifications")]
            SignalRConnectionInfo connectionInfo, FunctionContext context)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(connectionInfo);
            return response;
        }

        [Function("NotifyClients")]
        [SignalROutput(HubName = "notifications", ConnectionStringSetting = "AzureSignalRConnectionString")]
        public async Task<SignalRMessage> Run([EventGridTrigger] EventGridEvent eventGridEvent,
            FunctionContext context
        )
        {
            var @event = await _eventGridMessageDeserializer.DeserializeAsync(eventGridEvent);
            return new SignalRMessage
            {
                Target = @event.GetType()
                    .Name,
                Arguments = new object[] {@event}
            };
        }
    }
}