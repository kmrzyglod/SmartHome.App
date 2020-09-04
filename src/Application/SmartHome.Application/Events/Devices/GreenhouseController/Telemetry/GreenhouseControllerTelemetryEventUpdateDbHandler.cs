using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHome.Application.Shared.Events.Devices.GreenhouseController.Telemetry;
using SmartHome.Application.Shared.Events.Devices.WeatherStation.Telemetry;

namespace SmartHome.Application.Events.Devices.GreenhouseController.Telemetry
{
    public class GreenhouseControllerTelemetryEventUpdateDbHandler: INotificationHandler<GreenhouseControllerTelemetryEvent>
    {
        public Task Handle(GreenhouseControllerTelemetryEvent notification, CancellationToken cancellationToken) => throw new NotImplementedException();
    }
}
