using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace SmartHome.Integrations.Functions.HandleCommand
{
    public static class HandleCommand
    {
        [FunctionName("HandleCommand")]
        public static void Run([ServiceBusTrigger("commands", Connection = "ServiceBusConnectionString")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
