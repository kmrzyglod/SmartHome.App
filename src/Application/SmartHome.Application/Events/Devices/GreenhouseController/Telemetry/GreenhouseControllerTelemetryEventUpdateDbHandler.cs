using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHome.Application.Helpers;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Events.Devices.GreenhouseController.Telemetry;
using SmartHome.Domain.Entities.Devices.Greenhouse;

namespace SmartHome.Application.Events.Devices.GreenhouseController.Telemetry
{
    public class
        GreenhouseControllerTelemetryEventUpdateDbHandler : INotificationHandler<GreenhouseControllerTelemetryEvent>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public GreenhouseControllerTelemetryEventUpdateDbHandler(IApplicationDbContext applicationDbContext) =>
            _applicationDbContext = applicationDbContext;

        public Task Handle(GreenhouseControllerTelemetryEvent notification, CancellationToken cancellationToken)
        {
            AddAirParametersEntry(notification);
            AddInsolationParametersEntry(notification);
            AddGreenhouseSoilParametersEntry(notification);

            return _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        private void AddAirParametersEntry(GreenhouseControllerTelemetryEvent notification)
        {
            _applicationDbContext.GreenhouseAirParameters.Add(new GreenhouseAirParameters
            {
                Humidity = notification.Humidity.ToFixed(),
                Pressure = notification.Pressure.ToFixed(),
                Temperature = notification.Temperature.ToFixed(),
                Timestamp = notification.Timestamp
            });
        }

        private void AddInsolationParametersEntry(GreenhouseControllerTelemetryEvent notification)
        {
            _applicationDbContext.GreenhouseInsolationParameters.Add(new GreenhouseInsolationParameters
            {
                LightLevelInLux = notification.LightLevel,
                Timestamp = notification.Timestamp
            });
        }

        private void AddGreenhouseSoilParametersEntry(GreenhouseControllerTelemetryEvent notification)
        {
            _applicationDbContext.GreenhouseSoilParameters.Add(new GreenhouseSoilParameters
            {
                SoilMoisture = notification.SoilMoisture,
                Timestamp = notification.Timestamp
            });
        }
    }
}