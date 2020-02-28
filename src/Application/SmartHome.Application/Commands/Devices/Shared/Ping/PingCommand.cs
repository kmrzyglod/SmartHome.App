using System;
using MediatR;
using SmartHome.Application.Interfaces.Command;

namespace SmartHome.Application.Commands.Devices.Shared.Ping
{
    public class PingCommand : IRequest, IDeviceCommand
    {
        public Guid CorrelationId { get; set; }
        public string TargetDeviceId { get; set; } = string.Empty;
    }
}