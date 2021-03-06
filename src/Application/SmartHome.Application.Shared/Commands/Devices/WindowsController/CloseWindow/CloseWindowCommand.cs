﻿using System;
using MediatR;
using SmartHome.Application.Shared.Interfaces.Command;

namespace SmartHome.Application.Shared.Commands.Devices.WindowsController.CloseWindow
{
    public class CloseWindowCommand: IRequest, IDeviceCommand
    {
        public Guid CorrelationId { get; set; }
        public string TargetDeviceId { get; set; } = string.Empty;
        public ushort WindowId { get; set; }
        public string CommandName => nameof(CloseWindowCommand);
    }
}
