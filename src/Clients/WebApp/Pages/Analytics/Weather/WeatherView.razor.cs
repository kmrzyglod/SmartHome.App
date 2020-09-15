using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.Queries.WeatherStation.GetHumidity;
using SmartHome.Application.Shared.Queries.WeatherStation.GetPrecipitation;
using SmartHome.Application.Shared.Queries.WeatherStation.GetPressure;
using SmartHome.Application.Shared.Queries.WeatherStation.GetTemperature;
using SmartHome.Application.Shared.Queries.WeatherStation.GetWindParameters;
using SmartHome.Clients.WebApp.Services.Analytics;
using SmartHome.Clients.WebApp.Services.Logger;
using SmartHome.Clients.WebApp.Shared.Components.DateRangePicker;

namespace SmartHome.Clients.WebApp.Pages.Analytics.Weather
{
    public class WeatherViewModel : ComponentBase
    {
        protected DateRangeGranulation CurrentHumidityGranulation = DateRangeGranulation.Hour;
        protected DateRangeGranulation CurrentPressureGranulation = DateRangeGranulation.Hour;
        protected DateRangeGranulation CurrentTemperatureGranulation = DateRangeGranulation.Hour;
        protected DateRangeGranulation CurrentWindGranulation = DateRangeGranulation.Hour;
        protected DateRangeGranulation CurrentPrecipitationGranulation = DateRangeGranulation.Hour;

        protected DateTime DefaultFromDateTime;
        protected DateTime DefaultToDateTime;
        protected DateRangeGranulation DefaultGranulation = DateRangeGranulation.Hour;

        [Inject] protected IWeatherService _weatherService { get; set; }

        [Inject] protected IDateTimeProvider _dateTimeProvider { get; set; }

        protected List<TemperatureVm> TemperatureData { get; set; } = new List<TemperatureVm>();
        protected List<HumidityVm> HumidityData { get; set; } = new List<HumidityVm>();
        protected List<PressureVm> PressureData { get;  set; } = new List<PressureVm>();
        protected List<WindParametersVm> WindData { get;  set; } = new List<WindParametersVm>();
        protected List<PrecipitationVm> PrecipitationData { get; set; } = new List<PrecipitationVm>();

        protected async Task OnTemperaturesDatesRangeChanged(DateChangedEventArgs eventArgs)
        {
            CurrentTemperatureGranulation = eventArgs.Granulation;
            TemperatureData.Clear();
            TemperatureData.AddRange(await _weatherService.GetTemperature(new GetTemperatureQuery
            {
                From = eventArgs.FromDate,
                To = eventArgs.ToDate,
                Granulation = eventArgs.Granulation
            }));
        }

        protected async Task OnHumidityDatesRangeChanged(DateChangedEventArgs eventArgs)
        {
            CurrentHumidityGranulation = eventArgs.Granulation;
            HumidityData.Clear();
            HumidityData.AddRange(await _weatherService.GetHumidity(new GetHumidityQuery
            {
                From = eventArgs.FromDate,
                To = eventArgs.ToDate,
                Granulation = eventArgs.Granulation
            }));
        }

        protected async Task OnPressureDatesRangeChanged(DateChangedEventArgs eventArgs)
        {
            CurrentPressureGranulation = eventArgs.Granulation;
            PressureData.Clear();
            PressureData.AddRange(await _weatherService.GetPressure(new GetPressureQuery
            {
                From = eventArgs.FromDate,
                To = eventArgs.ToDate,
                Granulation = eventArgs.Granulation
            }));
        }

        protected async Task OnWindDatesRangeChanged(DateChangedEventArgs eventArgs)
        {
            CurrentWindGranulation = eventArgs.Granulation;
            WindData.Clear();
            WindData.AddRange(await _weatherService.GetWindParameters(new GetWindParametersQuery
            {
                From = eventArgs.FromDate,
                To = eventArgs.ToDate,
                Granulation = eventArgs.Granulation
            }));
        }

        protected async Task OnPrecipitationDatesRangeChanged(DateChangedEventArgs eventArgs)
        {
            CurrentPrecipitationGranulation = eventArgs.Granulation;
            PrecipitationData.Clear();
            PrecipitationData.AddRange(await _weatherService.GetPrecipitation(new GetPrecipitationQuery
            {
                From = eventArgs.FromDate,
                To = eventArgs.ToDate,
                Granulation = eventArgs.Granulation
            }));
        }

        protected override void OnInitialized()
        {
            DefaultFromDateTime = _dateTimeProvider.GetUtcNow().Date.AddDays(-2);
            DefaultToDateTime = _dateTimeProvider.GetUtcNow().Date.AddDays(1);
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var temperatureTask = _weatherService.GetTemperature(new GetTemperatureQuery{From = DefaultFromDateTime, To = DefaultToDateTime, Granulation = DefaultGranulation});
                var humidityTask = _weatherService.GetHumidity(new GetHumidityQuery{From = DefaultFromDateTime, To = DefaultToDateTime, Granulation = DefaultGranulation});
                var pressureTask = _weatherService.GetPressure(new GetPressureQuery{From = DefaultFromDateTime, To = DefaultToDateTime, Granulation = DefaultGranulation});
                var windTask = _weatherService.GetWindParameters(new GetWindParametersQuery{From = DefaultFromDateTime, To = DefaultToDateTime, Granulation = DefaultGranulation});
                var precipitationTask = _weatherService.GetPrecipitation(new GetPrecipitationQuery(){From = DefaultFromDateTime, To = DefaultToDateTime, Granulation = DefaultGranulation});
                await Task.WhenAll(temperatureTask, humidityTask, pressureTask, windTask, precipitationTask);

                TemperatureData.AddRange(await temperatureTask);
                HumidityData.AddRange(await humidityTask);
                PressureData.AddRange(await pressureTask);
                WindData.AddRange(await windTask);
                PrecipitationData.AddRange(await precipitationTask);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Failed to load weather data");
            }
        }

        protected string FormatArgument(DateTime date, DateRangeGranulation granulation)
        {
            return granulation switch
            {
                DateRangeGranulation.Raw => date.ToString("yyyy-MM-dd HH:mm"),
                DateRangeGranulation.FifteenMinutes => date.ToString("yyyy-MM-dd HH:mm"),
                DateRangeGranulation.HalfHour => date.ToString("yyyy-MM-dd HH:mm"),
                DateRangeGranulation.Hour => date.ToString("yyyy-MM-dd HH:mm"),
                DateRangeGranulation.ThreeHours => date.ToString("yyyy-MM-dd HH:mm"),
                DateRangeGranulation.SixHours => date.ToString("yyyy-MM-dd HH:mm"),
                DateRangeGranulation.Day => date.ToString("yyyy-MM-dd"),
                DateRangeGranulation.Month => date.ToString("yyyy-MM"),
                DateRangeGranulation.Year => date.ToString("yyyy"),
                _ => date.ToString("yyyy-MM-dd HH:mm")
            };
        }
    }
}