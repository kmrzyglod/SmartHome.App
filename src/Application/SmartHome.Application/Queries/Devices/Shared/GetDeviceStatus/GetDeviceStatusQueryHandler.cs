using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Interfaces.DbContext;

namespace SmartHome.Application.Queries.Devices.Shared.GetDeviceStatus
{
    public class GetDeviceStatusQueryHandler : IRequestHandler<GetDeviceStatusQuery, DeviceStatusVm?>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public GetDeviceStatusQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<DeviceStatusVm?> Handle(GetDeviceStatusQuery request, CancellationToken cancellationToken)
        {
            var deviceStatus = await _applicationDbContext.Device
                .Where(x => x.DeviceId == request.DeviceId)
                .Where(x => !x.IsDeleted)
                .Select(x => new
                {
                    x.DeviceName,
                    x.IsOnline,
                    x.LastStatusUpdate,
                    LastStatus = x.DeviceStatusHistory.OrderByDescending(s => s.Timestamp).FirstOrDefault()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (deviceStatus == null)
            {
                return null;
            }

            return new DeviceStatusVm
            {
                Ssid = deviceStatus.LastStatus?.Ssid,
                IsOnline = deviceStatus.IsOnline,
                FreeHeapMemory = deviceStatus.LastStatus?.FreeHeapMemory,
                Rssi = deviceStatus.LastStatus?.Rssi,
                Ip = deviceStatus.LastStatus?.Ip,
                GatewayIp = deviceStatus.LastStatus?.GatewayIp,
                DeviceName = deviceStatus.DeviceName,
                LastStatusUpdate = deviceStatus.LastStatusUpdate
            };
        }
    }
}