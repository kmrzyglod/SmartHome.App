using MediatR;
using SmartHome.Application.Models;

namespace SmartHome.Application.Queries.Devices.Shared.GetDeviceList
{
    public class GetDeviceListQuery: PageRequest, IRequest<PaginationResult<DeviceListEntryVm>>
    {
    }
}
