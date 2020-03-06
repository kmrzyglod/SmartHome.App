using MediatR;
using SmartHome.Application.Models;

namespace SmartHome.Application.Queries.Devices.Shared.GetDeviceStatusHistory
{
    public class GetDeviceStatusHistoryQuery: PageRequest, IRequest<PaginationResult<DeviceStatusHistoryVm>>
    {
        //Filters
        public string DeviceId { get; set; } = string.Empty;
    }
}
