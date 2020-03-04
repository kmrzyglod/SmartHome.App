﻿using System;

namespace SmartHome.Infrastructure.Configuration
{
    public class ConfigProvider : IConfigProvider
    {
        public string ApplicationDbConnectionString =>
            Environment.GetEnvironmentVariable("ApplicationDbConnectionString");
        public string IotHubConnectionString => Environment.GetEnvironmentVariable("IotHubConnectionString");
        public string CosmosDbConnectionString => Environment.GetEnvironmentVariable("CosmosDbConnectionString");
        public string ServiceBusConnectionString => Environment.GetEnvironmentVariable("ServiceBusConnectionString");
        public string CommandsQueueName => Environment.GetEnvironmentVariable("CommandsQueueName");
        public string SignalRConnectionString => Environment.GetEnvironmentVariable("AzureSignalRConnectionString");
        public string EventStoreContainer => Environment.GetEnvironmentVariable("EventStoreContainer");
        public string EventStoreDatabase => Environment.GetEnvironmentVariable("EventStoreDatabase");
    }
}