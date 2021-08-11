using System;

namespace SmartHome.Infrastructure.Configuration
{
    public class ConfigProvider : IConfigProvider
    {
        public string ApplicationDbConnectionString =>
            Environment.GetEnvironmentVariable("ApplicationDbConnectionString") ?? string.Empty;

        public string IotHubConnectionString =>
            Environment.GetEnvironmentVariable("IotHubConnectionString") ?? string.Empty;

        public string CosmosDbConnectionString =>
            Environment.GetEnvironmentVariable("CosmosDbConnectionString") ?? string.Empty;

        public string ServiceBusConnectionString =>
            Environment.GetEnvironmentVariable("ServiceBusConnectionString") ?? string.Empty;

        public string CommandsQueueName => Environment.GetEnvironmentVariable("CommandsQueueName") ?? string.Empty;

        public string SignalRConnectionString =>
            Environment.GetEnvironmentVariable("AzureSignalRConnectionString") ?? string.Empty;

        public string EventStoreContainer => Environment.GetEnvironmentVariable("EventStoreContainer") ?? string.Empty;
        public string EventStoreDatabase => Environment.GetEnvironmentVariable("EventStoreDatabase") ?? string.Empty;

        public string NotificationServiceUrl =>
            Environment.GetEnvironmentVariable("NotificationServiceUrl") ?? string.Empty;

        public string ApiHealthCheckEndpointUrl =>
            Environment.GetEnvironmentVariable("ApiHealthCheckEndpointUrl") ?? string.Empty;

        public string EventGridEndpointUrl =>
            Environment.GetEnvironmentVariable("EventGridEndpointUrl") ?? string.Empty;
        
        public string EventGridAuthKey =>
            Environment.GetEnvironmentVariable("EventGridAuthKey") ?? string.Empty;
        
        public string SendGridAuthKey =>
            Environment.GetEnvironmentVariable("SendGridAuthKey") ?? string.Empty;
    }
}