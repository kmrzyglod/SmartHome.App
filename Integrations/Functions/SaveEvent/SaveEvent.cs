using System.Net.Http;
using System.Text;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

namespace SmartHome.Integrations.Functions.SaveEvent
{
    public static class SaveEvent
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("SaveEvent")]
        public static void Run([IoTHubTrigger("messages/events", Connection = "IotHubConnectionString", ConsumerGroup = "saveevent")]EventData message, ILogger log)
        {
            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");
        }
    }
}