using System;
using MediatR;
using SmartHome.Application.Interfaces.Command;

namespace SmartHome.Application.Commands.Devices.Shared.SendDiagnosticData
{
    public class SendDiagnosticDataCommand : IRequest, IDeviceCommand
    {
        public Guid CorrelationId { get; set; }
        public string TargetDeviceId { get; set; }
    }
}