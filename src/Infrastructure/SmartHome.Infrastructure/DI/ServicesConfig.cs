using System;
using System.Reflection;
using MediatR;
using Microsoft.Azure.Devices;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SmartHome.Application.Events.App;
using SmartHome.Application.Interfaces.CommandBus;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Interfaces.DeviceCommandBus;
using SmartHome.Application.Interfaces.EventStore;
using SmartHome.Application.Interfaces.HttpClient;
using SmartHome.Application.Interfaces.NotificationService;
using SmartHome.Application.Shared.Events.App;
using SmartHome.Application.Shared.Interfaces.Cache;
using SmartHome.Application.Shared.Interfaces.Command;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.Interfaces.Event;
using SmartHome.Infrastructure.Cache;
using SmartHome.Infrastructure.CommandBusMessageDeserializer;
using SmartHome.Infrastructure.Configuration;
using SmartHome.Infrastructure.EventBusMessageDeserializer;
using SmartHome.Infrastructure.EventStore;
using SmartHome.Infrastructure.HttpClients;
using SmartHome.Infrastructure.MediatR;
using SmartHome.Infrastructure.NotificationService;
using SmartHome.Infrastructure.Persistence;
using SmartHome.Infrastructure.Shared.DateTimeProvider;

namespace SmartHome.Infrastructure.DI
{
    public static class ServicesConfig
    {
        private static readonly Assembly _eventTypesAssembly = typeof(IEvent).Assembly;
        private static readonly Assembly _commandTypesAssembly = typeof(ICommand).Assembly;

        public static IServiceCollection AddFramework(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<ICache, CustomMemoryCache>();
            return services;
        }

        public static IServiceCollection AddNotificationService(this IServiceCollection services)
        {
            services.AddHttpClient<INotificationServiceClient, NotificationServiceClient>((factory, client) =>
                client.BaseAddress = new Uri(factory.GetService<IConfigProvider>().NotificationServiceUrl));
            return services;
        }

        public static IServiceCollection AddHealthCheckService(this IServiceCollection services)
        {
            services.AddHttpClient<IApiHealthCheckHttpClient, ApiHealthCheckHttpClient>((factory, client) =>
                client.BaseAddress = new Uri(factory.GetService<IConfigProvider>().ApiHealthCheckEndpointUrl));
            return services;
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IConfigProvider, ConfigProvider>();
            return services;
        }

        public static IServiceCollection AddEventGridMessageHandling(this IServiceCollection services)
        {
            services.AddSingleton<IEventGridMessageDeserializer>(new EventGridMessageDeserializer(_eventTypesAssembly));
            return services;
        }

        public static IServiceCollection InitMediatR(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddMediatR(assemblies);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            return services;
        }

        public static IServiceCollection AddDeviceCommandBus(this IServiceCollection services)
        {
            services.AddSingleton(factory =>
            {
                var configProvider = factory.GetService<IConfigProvider>();
                return ServiceClient.CreateFromConnectionString(configProvider.IotHubConnectionString);
            });
            services.AddSingleton<IDeviceCommandBus, DeviceCommandBus.DeviceCommandBus>();
            return services;
        }

        public static IServiceCollection AddServiceBusMessageHandling(this IServiceCollection services)
        {
            services.AddSingleton<IServiceBusMessageDeserializer>(
                new ServiceBusMessageDeserializer(_commandTypesAssembly));
            return services;
        }

        public static IServiceCollection AddCommandBus(this IServiceCollection services)
        {
            services.AddSingleton<IQueueClient>(factory =>
            {
                var configProvider = factory.GetService<IConfigProvider>();
                return new QueueClient(configProvider.ServiceBusConnectionString, configProvider.CommandsQueueName);
            });
            services.AddSingleton<ICommandBus, CommandBus.CommandBus>();

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

        public static IServiceCollection AddApplicationDatabase(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>((factory, options) =>
            {
                var configProvider = factory.GetService<IConfigProvider>();
                options.UseSqlServer(
                    configProvider.ApplicationDbConnectionString,
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            return services;
        }
    }
}