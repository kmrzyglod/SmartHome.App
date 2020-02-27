using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Domain.Entities.Devices.WeatherStation;

namespace SmartHome.Application.Events.Devices.WeatherStation.Telemetry
{
    public class TelemetryEventUpdateDbHandler : INotificationHandler<TelemetryEvent>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public TelemetryEventUpdateDbHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public Task Handle(TelemetryEvent @event, CancellationToken cancellationToken)
        {
            AddAirEntry(@event);
            AddPrecipitationEntry(@event);
            AddSunEntry(@event);
            AddWindEntry(@event);

            return _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        private void AddAirEntry(TelemetryEvent @event)
        {
            _applicationDbContext.WeatherStationAir.Add(new Air
            {
                Humidity = @event.Humidity,
                Pressure = @event.Pressure,
                Temperature = @event.Temperature,
                MeasurementStartTime = @event.MeasurementStartTime,
                MeasurementEndTime = @event.MeasurementEndTime
            });
        }

        private void AddPrecipitationEntry(TelemetryEvent @event)
        {
            _applicationDbContext.WeatherStationPrecipitation.Add(new Precipitation
            {
                Rain = @event.Precipitation,
                MeasurementStartTime = @event.MeasurementStartTime,
                MeasurementEndTime = @event.MeasurementEndTime
            });
        }

        private void AddSunEntry(TelemetryEvent @event)
        {
            _applicationDbContext.WeatherStationSun.Add(new Sun
            {
                LightLevelInLux = @event.LightLevel,
                MeasurementStartTime = @event.MeasurementStartTime,
                MeasurementEndTime = @event.MeasurementEndTime
            });
        }

        private void AddWindEntry(TelemetryEvent @event)
        {
            _applicationDbContext.WeatherStationWind.Add(new Wind
            {
                MinWindSpeed = @event.MinWindSpeed,
                MaxWindSpeed = @event.MaxWindSpeed,
                AverageWindSpeed = @event.AverageWindSpeed,
                CurrentWindDirection = @event.CurrentWindDirection,
                MostFrequentWindDirection = @event.MostFrequentWindDirection,
                MeasurementStartTime = @event.MeasurementStartTime,
                MeasurementEndTime = @event.MeasurementEndTime
            });
        }
    }
}