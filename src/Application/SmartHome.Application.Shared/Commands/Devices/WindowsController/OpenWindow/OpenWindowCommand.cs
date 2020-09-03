using System;
using MediatR;
using SmartHome.Application.Shared.Interfaces.Command;

namespace SmartHome.Application.Shared.Commands.Devices.WindowsController.OpenWindow
{
    public class OpenWindowCommand: IRequest, IDeviceCommand
    {
        public Guid CorrelationId { get; set; }
        public string TargetDeviceId { get; set; } = string.Empty;
        public ushort WindowId { get; set; }
    }
}
