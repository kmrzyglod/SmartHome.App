using System.Threading.Tasks;
using SmartHome.Application.Shared.Models;
using SmartHome.Application.Shared.Queries.General.GetDeviceList;

namespace SmartHome.Clients.WebApp.Services.Devices
{
    public interface IDevicesService
    {
        Task<PaginationResult<DeviceListEntryVm>> GetDevicesList(GetDeviceListQuery query, bool withCache = true);
    }
}