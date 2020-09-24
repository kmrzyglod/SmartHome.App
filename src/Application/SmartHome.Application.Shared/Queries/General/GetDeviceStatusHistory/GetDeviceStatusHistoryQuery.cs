using MediatR;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.General.GetDeviceStatusHistory
{
    public class GetDeviceStatusHistoryQuery: PageRequest, IRequest<PaginationResult<DeviceStatusHistoryVm>>
    {
        //Filters
        public string DeviceId { get; set; } = string.Empty;
    }
}
