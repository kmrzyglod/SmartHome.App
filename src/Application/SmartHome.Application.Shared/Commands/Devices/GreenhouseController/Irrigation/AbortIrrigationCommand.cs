using System;
using MediatR;
using SmartHome.Application.Shared.Interfaces.Command;

namespace SmartHome.Application.Shared.Commands.Devices.GreenhouseController.Irrigation
{
    public class AbortIrrigationCommand: IRequest, IDeviceCommand
    {
        public Guid CorrelationId { get; set; }
        public string TargetDeviceId { get; set; } = null!;
    }
}
