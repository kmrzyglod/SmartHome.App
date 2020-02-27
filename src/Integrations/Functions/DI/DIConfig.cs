using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using SmartHome.Application.Commands.Devices.Shared.Ping;
using SmartHome.Infrastructure.DI;
using SmartHome.Integrations.Functions.DI;

[assembly: FunctionsStartup(typeof(DIConfig))]
namespace SmartHome.Integrations.Functions.DI
{
    public class DIConfig : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var mediatrHandlersAssembly = typeof(PingCommand).Assembly;

            builder.Services
                .AddFramework()
                .AddConfiguration()
                .AddEventGridMessageHandling()
                .AddEventStoreClient()
                .AddApplicationDatabase()
                .AddMediatR(mediatrHandlersAssembly);
        }
    }
}