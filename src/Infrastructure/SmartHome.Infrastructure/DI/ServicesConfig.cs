using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SmartHome.Application.Interfaces.DateTime;
using SmartHome.Application.Interfaces.EventStore;
using SmartHome.Infrastructure.Configuration;
using SmartHome.Infrastructure.DateTime;
using SmartHome.Infrastructure.DeviceEventDeserializer;
using SmartHome.Infrastructure.EventStore;

namespace SmartHome.Infrastructure.DI
{
    public static class ServicesConfig
    {
        public static IServiceCollection AddFramework(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            return services;
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IConfigProvider, ConfigProvider>();
            return services;
        }

        public static IServiceCollection AddEventGridMessageHandling(this IServiceCollection services)
        {
            services.AddSingleton<IEventGridMessageDeserializer, EventGridMessageDeserializer>();
            return services;
        }

        public static IServiceCollection AddEventStoreClient(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(factory =>
            {
                var configProvider = factory.GetService<IConfigProvider>();
                return new MongoClient(configProvider.CosmosDbConnectionString);
            });

            services.AddTransient(factory =>
            {
                var mongoClient = factory.GetService<IMongoClient>();
                var configProvider = factory.GetService<IConfigProvider>();
                return mongoClient.GetDatabase(configProvider.EventStoreDatabase);
            });

            services.AddSingleton<IEventStoreClient, EventStoreClient>();
            return services;
        }
    }
}