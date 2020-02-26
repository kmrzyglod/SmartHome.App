namespace SmartHome.Infrastructure.Configuration
{
    public interface IConfigProvider
    {
        string GreenhouseControllerDbConnectionString { get; }
        string WeatherStationDbConnectionString { get; }
        string IotHubConnectionString { get; }
        string CosmosDbConnectionString { get; }
        string ServiceBusConnectionString { get; }
        string SignalRConnectionString { get; }
        string EventStoreContainer { get; }
        string EventStoreDatabase { get; }
    }
}