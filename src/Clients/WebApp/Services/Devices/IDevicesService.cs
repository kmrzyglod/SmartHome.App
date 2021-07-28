using System.Threading.Tasks;
using SmartHome.Application.Shared.Commands.Devices.Shared.Ping;
using SmartHome.Application.Shared.Commands.Devices.Shared.SendDiagnosticData;
using SmartHome.Application.Shared.Models;
using SmartHome.Application.Shared.Queries.General.GetDeviceList;
using SmartHome.Application.Shared.Queries.General.GetDeviceStatus;

namespace SmartHome.Clients.WebApp.Services.Devices
{
    public interface IDevicesService
    {
        Task<PaginationResult<DeviceListEntryVm>> GetDevicesList(GetDeviceListQuery query, bool withCache = true);
        Task<DeviceStatusVm> GetDeviceStatus(GetDeviceStatusQuery query, bool withCache = true);
        Task<CommandCorrelationId> Ping(PingCommand command);
        Task<CommandCorrelationId> SendDiagnosticData(SendDiagnosticDataCommand command);
    }
}