using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetHumidity;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetInsolation;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetIrrigationData;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetSoilMoisture;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetTemperature;
using SmartHome.Clients.WebApp.Services.Analytics;
using SmartHome.Clients.WebApp.Services.Logger;
using SmartHome.Clients.WebApp.Shared.Components.DateRangePicker;

namespace SmartHome.Clients.WebApp.Pages.Analytics.Greenhouse
{
    public class GreenhouseViewModel : ComponentBase
    {
        protected DateRangeGranulation CurrentTemperatureGranulation = DateRangeGranulation.Hour;
        protected DateRangeGranulation CurrentHumidityGranulation = DateRangeGranulation.Hour;
        protected DateRangeGranulation CurrentInsolationGranulation = DateRangeGranulation.Hour;
        protected DateRangeGranulation CurrentSoilMoistureGranulation = DateRangeGranulation.Hour;
        protected DateRangeGranulation CurrentIrrigationDataGranulation = DateRangeGranulation.Hour;

        protected DateTime DefaultFromDateTime;
        protected DateTime DefaultToDateTime;
        protected DateRangeGranulation DefaultGranulation = DateRangeGranulation.Hour;

        [Inject] protected IGreenhouseService _greenhouseService { get; set; }

        [Inject] protected IDateTimeProvider _dateTimeProvider { get; set; }

        protected IEnumerable<TemperatureVm> TemperatureData { get; set; } = new List<TemperatureVm>();
        protected IEnumerable<HumidityVm> HumidityData { get; set; } = new List<HumidityVm>();
        protected IEnumerable<InsolationVm> InsolationData { get; set; } = new List<InsolationVm>();
        protected IEnumerable<SoilMoistureVm> SoilMoistureData { get; set; } = new List<SoilMoistureVm>();
        protected IEnumerable<IrrigationDataVm> IrrigationData { get; set; } = new List<IrrigationDataVm>();

        protected async Task OnTemperaturesDatesRangeChanged(DateChangedEventArgs eventArgs)
        {
            CurrentTemperatureGranulation = eventArgs.Granulation;
            TemperatureData = Enumerable.Empty<TemperatureVm>();
            TemperatureData = await _greenhouseService.GetTemperature(new GetTemperatureQuery
            {
                From = eventArgs.FromDate,
                To = eventArgs.ToDate,
                Granulation = eventArgs.Granulation
            });
        }

        protected async Task OnHumidityDatesRangeChanged(DateChangedEventArgs eventArgs)
        {
            CurrentHumidityGranulation = eventArgs.Granulation;
            HumidityData = await _greenhouseService.GetHumidity(new GetHumidityQuery
            {
                From = eventArgs.FromDate,
                To = eventArgs.ToDate,
                Granulation = eventArgs.Granulation
            });
        }

        protected async Task OnInsolationDatesRangeChanged(DateChangedEventArgs eventArgs)
        {
            CurrentInsolationGranulation = eventArgs.Granulation;
            InsolationData = await _greenhouseService.GetInsolation(new GetInsolationQuery
            {
                From = eventArgs.FromDate,
                To = eventArgs.ToDate,
                Granulation = eventArgs.Granulation
            });
        }

        protected async Task OnSoilMoistureDatesRangeChanged(DateChangedEventArgs eventArgs)
        {
            CurrentSoilMoistureGranulation = eventArgs.Granulation;
            SoilMoistureData = await _greenhouseService.GetSoilMoisture(new GetSoilMoistureQuery
            {
                From = eventArgs.FromDate,
                To = eventArgs.ToDate,
                Granulation = eventArgs.Granulation
            });
        }

        protected async Task OnIrrigationHistoryDatesRangeChanged(DateChangedEventArgs eventArgs)
        {
            CurrentIrrigationDataGranulation = eventArgs.Granulation;
            IrrigationData = await _greenhouseService.GetIrrigationData(new GetIrrigationDataQuery
            {
                From = eventArgs.FromDate,
                To = eventArgs.ToDate,
                Granulation = eventArgs.Granulation
            });
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
                var temperatureTask = _greenhouseService.GetTemperature(new GetTemperatureQuery{From = DefaultFromDateTime, To = DefaultToDateTime, Granulation = DefaultGranulation});
                var humidityTask = _greenhouseService.GetHumidity(new GetHumidityQuery{From = DefaultFromDateTime, To = DefaultToDateTime, Granulation = DefaultGranulation});
                var insolationTask = _greenhouseService.GetInsolation(new GetInsolationQuery{From = DefaultFromDateTime, To = DefaultToDateTime, Granulation = DefaultGranulation});
                var soilMoistureTask = _greenhouseService.GetSoilMoisture(new GetSoilMoistureQuery{From = DefaultFromDateTime, To = DefaultToDateTime, Granulation = DefaultGranulation});
                var irrigationDataTask = _greenhouseService.GetIrrigationData(new GetIrrigationDataQuery{From = DefaultFromDateTime, To = DefaultToDateTime, Granulation = DefaultGranulation});
                
                await Task.WhenAll(temperatureTask, humidityTask, insolationTask, soilMoistureTask, irrigationDataTask);

                TemperatureData = await temperatureTask;
                HumidityData = await humidityTask;
                InsolationData = await insolationTask;
                SoilMoistureData =  await soilMoistureTask;
                IrrigationData  = await irrigationDataTask;
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