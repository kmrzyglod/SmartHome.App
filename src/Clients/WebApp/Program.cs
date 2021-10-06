using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using MatBlazor;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Radzen;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Clients.WebApp.Services.Analytics;
using SmartHome.Clients.WebApp.Services.Devices;
using SmartHome.Clients.WebApp.Services.EventLog;
using SmartHome.Clients.WebApp.Services.Rules;
using SmartHome.Clients.WebApp.Services.Shared.ApiClient;
using SmartHome.Clients.WebApp.Services.Shared.CommandsExecutor;
using SmartHome.Clients.WebApp.Services.Shared.NotificationsHub;
using SmartHome.Infrastructure.Shared.DateTimeProvider;

namespace SmartHome.Clients.WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
           var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            //builder.Services.AddTransient(sp =>
            //    new HttpClient
            //    {
            //        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
            //    }.EnableIntercept(sp));

            builder.Services.AddHttpClient("SmartHome.Clients.WebApp.ServerAPI",
                    (sp,client) =>
                    {
                        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                        //client.EnableIntercept(sp);
                    })
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddSingleton(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("SmartHome.Clients.WebApp.ServerAPI"));

            builder.Services.AddLoadingBar();
            builder.Services.AddDevExpressBlazor();
            builder.Services.AddSingleton<NotificationService>();
            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddSingleton<IApiClient, ApiClient>();
            builder.Services.AddSingleton<IWeatherService, WeatherService>();
            builder.Services.AddSingleton<IGreenhouseService, GreenhouseService>();
            builder.Services.AddSingleton<IEventLogService, EventLogService>();
            builder.Services.AddSingleton<IDevicesService, DevicesService>();
            builder.Services.AddSingleton<IRulesService, RulesService>();
            builder.Services.AddSingleton<INotificationsHub, SignalRNotificationsHub>();
            builder.Services.AddSingleton<ICommandsExecutor, CommandsExecutor>();
            builder.Services.AddScoped<AppState>();
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddMatToaster(config =>
            {
                config.Position = MatToastPosition.BottomRight;
                config.PreventDuplicates = true;
                config.NewestOnTop = true;
                config.ShowCloseButton = true;
                config.MaximumOpacity = 95;
                config.VisibleStateDuration = 3000;
            });
            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                options.ProviderOptions.LoginMode = "redirect";
                options.ProviderOptions.DefaultAccessTokenScopes.Add("api://49e199cc-d31e-4911-bd3e-8b36dedf5ba1/Smart.Home.API.Access");
            });

            await builder
            .Build()
            .UseLoadingBar()
            .RunAsync();
        }
    }
}
