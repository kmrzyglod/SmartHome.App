using MediatR;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.GetDeviceList
{
    public class GetDeviceListQuery: PageRequest, IRequest<PaginationResult<DeviceListEntryVm>>
    {
    }
}
