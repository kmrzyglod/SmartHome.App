using MediatR;

namespace SmartHome.Application.Queries.Devices.Shared.GetDeviceStatus
{
    public class GetDeviceStatusQuery: IRequest<DeviceStatusVm>
    {
        public string DeviceId { get; set; } = string.Empty;
    }
}
