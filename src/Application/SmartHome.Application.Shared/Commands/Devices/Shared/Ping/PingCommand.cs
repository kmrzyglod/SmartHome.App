﻿using System;
using MediatR;
using SmartHome.Application.Shared.Interfaces.Command;

namespace SmartHome.Application.Shared.Commands.Devices.Shared.Ping
{
    public class PingCommand : IRequest, IDeviceCommand
    {
        public Guid CorrelationId { get; set; }
        public string TargetDeviceId { get; set; } = string.Empty;
        public string CommandName => nameof(PingCommand);
    }
}