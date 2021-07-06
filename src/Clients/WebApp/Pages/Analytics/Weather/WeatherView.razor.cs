using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SmartHome.Application.Shared.Events.Devices.WeatherStation.Telemetry;
using SmartHome.Clients.WebApp.Services.Analytics;
using SmartHome.Clients.WebApp.Services.Logger;
using SmartHome.Clients.WebApp.Services.Shared.NotificationsHub;
using SmartHome.Clients.WebApp.Shared.Components.HumidityChart;
using SmartHome.Clients.WebApp.Shared.Components.PrecipitationChart;
using SmartHome.Clients.WebApp.Shared.Components.PressureChart;
using SmartHome.Clients.WebApp.Shared.Components.TemperatureChart;
using SmartHome.Clients.WebApp.Shared.Components.WindChart;

namespace SmartHome.Clients.WebApp.Pages.Analytics.Weather
{
    public class WeatherViewModel : ComponentBase
    {
        [Inject] protected IWeatherService WeatherService { get; set; } = null!;
        [Inject] protected INotificationsHub NotificationsHub { get; set; } = null!;

        protected TemperatureChartComponent TemperatureChart { get; set; } = null!;
        protected HumidityChartComponent HumidityChart { get; set; } = null!;
        protected PressureChartComponent PressureChart { get; set; } = null!;
        protected WindChartComponent WindChart { get; set; } = null!;
        protected PrecipitationChartComponent PrecipitationChart { get; set; } = null!;

        protected override void OnInitialized()
        {
        }

        protected async Task OnTabChange(int index)
        {
            try
            {
                switch (index)
                {
                    case 0:
                        await TemperatureChart.UpdateData();
                        break;
                    case 1:
                        await HumidityChart.UpdateData();
                        break;
                    case 2:
                        await PressureChart.UpdateData();
                        break;
                    case 3:
                        await WindChart.UpdateData();
                        break;
                    case 4:
                        await PrecipitationChart.UpdateData();
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Failed to load weather data");
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                NotificationsHub.Subscribe<WeatherTelemetryEvent>(e => { Console.WriteLine(e.Temperature); });
                await NotificationsHub.ConnectAsync();
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Failed to load weather data");
            }
        }
    }
}