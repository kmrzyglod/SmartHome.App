using System;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
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
        private static readonly string _storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage") ?? string.Empty;

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
                        .InitMediatR(applicationSharedAssembly, applicationAssembly);

                    // Add Custom Services
                    services.AddSingleton(CloudStorageAccount.Parse(_storageConnectionString)
                        .CreateCloudBlobClient());
                })
                .Build();

            return host.RunAsync();
        }
    }
}
