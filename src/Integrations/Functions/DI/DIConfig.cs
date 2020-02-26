using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using SmartHome.Infrastructure.DI;
using SmartHome.Integrations.Functions.DI;

[assembly: FunctionsStartup(typeof(DIConfig))]
namespace SmartHome.Integrations.Functions.DI
{

    public class DIConfig : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddFramework()
                .AddConfiguration()
                .AddEventGridMessageHandling()
                .AddEventStoreClient();
        }
    }
}