namespace SmartHome.Infrastructure.Configuration
{
    public interface IConfigProvider
    {
        string ApplicationDbConnectionString { get; }
        string IotHubConnectionString { get; }
        string CosmosDbConnectionString { get; }
        string ServiceBusConnectionString { get; }
        string CommandsQueueName { get; }
        string SignalRConnectionString { get; }
        string EventStoreContainer { get; }
        string EventStoreDatabase { get; }
        string NotificationServiceUrl { get; }
        string ApiHealthCheckEndpointUrl { get; }
    }
}