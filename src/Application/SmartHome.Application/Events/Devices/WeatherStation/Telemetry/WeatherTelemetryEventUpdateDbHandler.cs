﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHome.Application.Helpers;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Events.Devices.WeatherStation.Telemetry;
using SmartHome.Domain.Entities.Devices.WeatherStation;

namespace SmartHome.Application.Events.Devices.WeatherStation.Telemetry
{
    public class WeatherTelemetryEventUpdateDbHandler : INotificationHandler<WeatherTelemetryEvent>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public WeatherTelemetryEventUpdateDbHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public Task Handle(WeatherTelemetryEvent @event, CancellationToken cancellationToken)
        {
            AddAirEntry(@event);
            AddPrecipitationEntry(@event);
            AddSunEntry(@event);
            AddWindEntry(@event);

            return _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        private void AddAirEntry(WeatherTelemetryEvent @event)
        {
            _applicationDbContext.WeatherStationAirParameters.Add(new WeatherStationAirParameters
            {
                Humidity = @event.Humidity.ToFixed(),
                Pressure = @event.Pressure.ToFixed(),
                Temperature = @event.Temperature.ToFixed(),
                MeasurementStartTime = @event.MeasurementStartTime,
                MeasurementEndTime = @event.MeasurementEndTime
            });
        }

        private void AddPrecipitationEntry(WeatherTelemetryEvent @event)
        {
            _applicationDbContext.WeatherStationPrecipitation.Add(new WeatherStationPrecipitation
            {
                Rain = @event.Precipitation.ToFixed(3),
                MeasurementStartTime = @event.MeasurementStartTime,
                MeasurementEndTime = @event.MeasurementEndTime
            });
        }

        private void AddSunEntry(WeatherTelemetryEvent @event)
        {
            _applicationDbContext.WeatherStationInsolationParameters.Add(new WeatherStationInsolationParameters
            {
                LightLevelInLux = @event.LightLevel,
                MeasurementStartTime = @event.MeasurementStartTime,
                MeasurementEndTime = @event.MeasurementEndTime
            });
        }

        private void AddWindEntry(WeatherTelemetryEvent @event)
        {
            _applicationDbContext.WeatherStationWindParameters.Add(new WeatherStationWindParameters
            {
                MinWindSpeed = @event.MinWindSpeed.ToFixed(),
                MaxWindSpeed = @event.MaxWindSpeed.ToFixed(),
                AverageWindSpeed = @event.AverageWindSpeed.ToFixed(),
                CurrentWindDirection = @event.CurrentWindDirection,
                MostFrequentWindDirection = @event.MostFrequentWindDirection,
                MeasurementStartTime = @event.MeasurementStartTime,
                MeasurementEndTime = @event.MeasurementEndTime
            });
        }
    }
}