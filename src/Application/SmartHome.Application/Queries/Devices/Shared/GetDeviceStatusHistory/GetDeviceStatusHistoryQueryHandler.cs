using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Extensions;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Models;

namespace SmartHome.Application.Queries.Devices.Shared.GetDeviceStatusHistory
{
    public class
        GetDeviceStatusHistoryQueryHandler : IRequestHandler<GetDeviceStatusHistoryQuery,
            PaginationResult<DeviceStatusHistoryVm>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public GetDeviceStatusHistoryQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<PaginationResult<DeviceStatusHistoryVm>> Handle(GetDeviceStatusHistoryQuery request,
            CancellationToken cancellationToken)
        {
            var query = _applicationDbContext.DeviceStatuses
                .Where(x => x.Device.DeviceId == request.DeviceId)
                .Where(x => !x.Device.IsDeleted);

            int count = await query.CountAsync(cancellationToken);

            var result = await query
                .OrderByDescending(x => x.Timestamp)
                .AddPagination(request.PageNumber, request.PageSize)
                .Select(x => new DeviceStatusHistoryVm
                {
                    GatewayIp = x.GatewayIp,
                    FreeHeapMemory = x.FreeHeapMemory,
                    Rssi = x.Rssi,
                    Ip = x.Ip,
                    Ssid = x.Ssid,
                    Timestamp = x.Timestamp,
                    DeviceId = x.Device.DeviceId
                })
                .ToListAsync(cancellationToken);

            return new PaginationResult<DeviceStatusHistoryVm>
            {
                PageSize = result.Count,
                PageNumber = request.PageNumber,
                ResultTotalCount = count,
                Result = result
            };
        }
    }
}