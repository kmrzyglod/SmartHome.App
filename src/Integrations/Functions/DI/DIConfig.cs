using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Interfaces.Command;
using SmartHome.Infrastructure.DI;
using SmartHome.Integrations.Functions.DI;

[assembly: FunctionsStartup(typeof(DIConfig))]
namespace SmartHome.Integrations.Functions.DI
{
    public class DIConfig : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var applicationAssembly = typeof(IApplicationDbContext).Assembly;
            var applicationSharedAssembly = typeof(ICommand).Assembly;

            builder.Services
                .AddLogging()
                .AddFramework()
                .AddConfiguration()
                .AddEventGridMessageHandling()
                .AddServiceBusMessageHandling()
                .AddEventStoreClient()
                .AddDeviceCommandBus()
                .AddApplicationDatabase()
                .InitMediatR(applicationSharedAssembly, applicationAssembly);
        }
    }
}