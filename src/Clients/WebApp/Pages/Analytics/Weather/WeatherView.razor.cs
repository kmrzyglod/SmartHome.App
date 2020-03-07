using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SmartHome.Application.Shared.Queries.GetHumidity;
using SmartHome.Application.Shared.Queries.GetPressure;
using SmartHome.Application.Shared.Queries.GetTemperature;
using SmartHome.Clients.WebApp.Services.Analytics;
using SmartHome.Clients.WebApp.Services.Shared.ApiClient;
using SmartHome.Clients.WebApp.Shared.Components.DateRangePicker;

namespace SmartHome.Clients.WebApp.Pages.Analytics.Weather
{
    public class WeatherViewModel: ComponentBase
    {
        [Inject]
        protected IWeatherService WeatherService { get; set; }

        protected IEnumerable<TemperatureVm> TemperatureData { get; private set;} = Enumerable.Empty<TemperatureVm>();
        protected IEnumerable<HumidityVm> HumidityData { get; private set;} = Enumerable.Empty<HumidityVm>();
        protected IEnumerable<PressureVm> PressureData { get; private set;} = Enumerable.Empty<PressureVm>();

        protected async Task OnTemperaturesDatesRangeChanged(DateChnagedEventArgs eventArgs)
        {
            TemperatureData = await WeatherService.GetTemperature(new GetTemperatureQuery
            {
                From = eventArgs.FromDate,
                To = eventArgs.ToDate
            });
        }

        protected async Task OnHumidityDatesRangeChanged(DateChnagedEventArgs eventArgs)
        {
            HumidityData = await WeatherService.GetHumidity(new GetHumidityQuery
            {
                From = eventArgs.FromDate,
                To = eventArgs.ToDate
            });
        }

        protected async Task OnPressureDatesRangeChanged(DateChnagedEventArgs eventArgs)
        {
            PressureData = await WeatherService.GetPressure(new GetPressureQuery
            {
                From = eventArgs.FromDate,
                To = eventArgs.ToDate
            });
        }

        protected override async Task OnInitializedAsync()
        {
            var temperatureTask = WeatherService.GetTemperature(new GetTemperatureQuery { });
            var humidityTask = WeatherService.GetHumidity(new GetHumidityQuery { });
            var pressureTask = WeatherService.GetPressure(new GetPressureQuery { });
            await Task.WhenAll(temperatureTask, humidityTask, pressureTask);

            TemperatureData = await temperatureTask;
            HumidityData = await humidityTask;
            PressureData = await pressureTask;
            await base.OnInitializedAsync();
        }
    }
}
