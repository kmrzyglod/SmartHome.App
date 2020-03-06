using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Extensions;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Models;

namespace SmartHome.Application.Queries.Devices.Shared.GetDeviceList
{
    public class GetDeviceListQueryHandler : IRequestHandler<GetDeviceListQuery, PaginationResult<DeviceListEntryVm>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public GetDeviceListQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<PaginationResult<DeviceListEntryVm>> Handle(GetDeviceListQuery request,
            CancellationToken cancellationToken)
        {
            var query = _applicationDbContext.Device
                .Where(x => !x.IsDeleted);

            int count = await query.CountAsync(cancellationToken);

            var result = await query
                .AddPagination(request.PageNumber, request.PageSize)
                .Select(x => new DeviceListEntryVm
                {
                    DeviceId = x.DeviceId,
                    DeviceName = x.DeviceName,
                    LastStatusUpdate = x.LastStatusUpdate,
                    IsOnline = x.IsOnline
                })
                .ToListAsync(cancellationToken);

            return new PaginationResult<DeviceListEntryVm>
            {
                PageSize = result.Count,
                PageNumber = request.PageNumber,
                ResultTotalCount = count,
                Result = result
            };
        }
    }
}