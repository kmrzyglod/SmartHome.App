﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using MatBlazor;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Clients.WebApp.Services.Analytics;
using SmartHome.Clients.WebApp.Services.Shared.ApiClient;
using SmartHome.Infrastructure.Shared.DateTimeProvider;

namespace SmartHome.Clients.WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
           var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddLoadingBar();
            builder.Services.AddDevExpressBlazor();
          
            builder.Services.AddLoadingBar();
            builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddSingleton<IApiClient, ApiClient>();
            builder.Services.AddSingleton<IWeatherService, WeatherService>();
            builder.Services.AddScoped<AppState>();
            builder.Services.AddMatToaster(config =>
            {
                config.Position = MatToastPosition.BottomRight;
                config.PreventDuplicates = true;
                config.NewestOnTop = true;
                config.ShowCloseButton = true;
                config.MaximumOpacity = 95;
                config.VisibleStateDuration = 3000;
            });

            await builder
            .Build()
            .UseLoadingBar()
            .RunAsync();
        }
    }
}
