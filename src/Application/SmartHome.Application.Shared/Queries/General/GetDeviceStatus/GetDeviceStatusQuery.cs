﻿using MediatR;

namespace SmartHome.Application.Shared.Queries.General.GetDeviceStatus
{
    public class GetDeviceStatusQuery: IRequest<DeviceStatusVm>
    {
        public string DeviceId { get; set; } = string.Empty;
    }
}
