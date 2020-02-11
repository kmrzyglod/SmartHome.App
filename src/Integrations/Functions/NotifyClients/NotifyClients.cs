using System.Net.Http;
using System.Text;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

namespace SmartHome.Integrations.Functions.NotifyClients
{
    public static class NotifyClients
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("NotifyClients")]
        public static void Run([IoTHubTrigger("messages/events", Connection = "IotHubConnectionString", ConsumerGroup = "notifyclients")]EventData message, ILogger log)
        {
            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");
        }
    }
}