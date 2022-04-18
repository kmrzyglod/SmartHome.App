using System;
using System.Net.Http;
using System.Threading.Tasks;
using MatBlazor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Clients.WebApp.Services.Analytics;
using SmartHome.Clients.WebApp.Services.Devices;
using SmartHome.Clients.WebApp.Services.EventLog;
using SmartHome.Clients.WebApp.Services.Rules;
using SmartHome.Clients.WebApp.Services.Shared.ApiClient;
using SmartHome.Clients.WebApp.Services.Shared.ApiUrlProvider;
using SmartHome.Clients.WebApp.Services.Shared.AuthTokenProvider;
using SmartHome.Clients.WebApp.Services.Shared.CommandsExecutor;
using SmartHome.Clients.WebApp.Services.Shared.Exceptions;
using SmartHome.Clients.WebApp.Services.Shared.NotificationsHub;
using SmartHome.Clients.WebApp.Shared.Authentication;
using SmartHome.Infrastructure.Shared.DateTimeProvider;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace SmartHome.Clients.WebApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("app");
        builder.Services.AddHttpClient("SmartHome.Clients.WebApp.ServerAPI",
                (sp, client) =>
                {
                    client.BaseAddress = new Uri($"{sp.GetRequiredService<IApiUrlProviderService>().GetApiUrl()}/");
                    client.EnableIntercept(sp);
                })
            .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient("SmartHome.Clients.WebApp.ServerAPI"));

        builder.Services.AddLoadingBar();
        builder.Services.AddDevExpressBlazor();
        builder.Services.AddSingleton<NotificationService>();
        builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        builder.Services.AddSingleton<ICustomExceptionsService, CustomExceptionsService>();
        builder.Services.AddScoped<CustomAuthorizationMessageHandler>();
        builder.Services.AddScoped<IApiClient, ApiClient>();
        builder.Services.AddScoped<IWeatherService, WeatherService>();
        builder.Services.AddScoped<IGreenhouseService, GreenhouseService>();
        builder.Services.AddScoped<IEventLogService, EventLogService>();
        builder.Services.AddScoped<IDevicesService, DevicesService>();
        builder.Services.AddScoped<IRulesService, RulesService>();
        builder.Services.AddScoped<INotificationsHub, SignalRNotificationsHub>();
        builder.Services.AddScoped<ICommandsExecutor, CommandsExecutor>();
        builder.Services.AddScoped<IAuthTokenProviderService, AuthTokenProviderService>();
        builder.Services.AddSingleton<IApiUrlProviderService, ApiUrlProviderService>();
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
        });

        await builder
            .UseLoadingBar()
            .Build()
            .RunAsync();
    }
}