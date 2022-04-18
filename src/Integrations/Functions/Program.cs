using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Interfaces.Command;
using SmartHome.Infrastructure.DI;
using SmartHome.Integrations.Functions.DI;

namespace SmartHome.Integrations.Functions
{
    internal class Program
    {
        private static Task Main(string[] args)
        {
            var applicationAssembly = typeof(IApplicationDbContext).Assembly;
            var applicationSharedAssembly = typeof(ICommand).Assembly;

            var host = new HostBuilder()
                .ConfigureAppConfiguration(configurationBuilder => { configurationBuilder.AddCommandLine(args); })
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    // Add Logging
                    services.AddLogging();

                    // Add HttpClient
                    services.AddHttpClient();

                    services.AddFramework()
                        .AddHealthCheckService()
                        .AddConfiguration()
                        .AddEventGridMessageHandling()
                        .AddServiceBusMessageHandling()
                        .AddEventStoreClient()
                        .AddDeviceCommandBus()
                        .AddApplicationDatabase()
                        .AddScheduledTasksProvider()
                        .AddEmailSender()
                        .AddRuleEngine()
                        .InitMediatR(applicationSharedAssembly, applicationAssembly);
                })
                .Build();

            return host.RunAsync();
        }
    }
}
