using System.Threading.Tasks;
using SmartHome.Application.Shared.Commands.Devices.Shared.Ping;
using SmartHome.Application.Shared.Commands.Devices.Shared.SendDiagnosticData;
using SmartHome.Application.Shared.Models;
using SmartHome.Application.Shared.Queries.General.GetDeviceList;
using SmartHome.Application.Shared.Queries.General.GetDeviceStatus;
using SmartHome.Clients.WebApp.Services.Shared.ApiClient;

namespace SmartHome.Clients.WebApp.Services.Devices
{
    public class DevicesService : IDevicesService
    {
        private readonly IApiClient _apiClient;

        public DevicesService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public Task<PaginationResult<DeviceListEntryVm>> GetDevicesList(GetDeviceListQuery query, bool withCache = true)
        {
            return _apiClient.Get<GetDeviceListQuery, PaginationResult<DeviceListEntryVm>>("Devices/list", query,
                withCache ? null : _apiClient.NoCacheHeader);
        }

        public Task<DeviceStatusVm> GetDeviceStatus(GetDeviceStatusQuery query, bool withCache = true)
        {
            return _apiClient.Get<GetDeviceStatusQuery, DeviceStatusVm>("Devices/status", query,
                withCache ? null : _apiClient.NoCacheHeader);
        }

        public Task<CommandCorrelationId> Ping(PingCommand command)
        {
            return _apiClient.Post<PingCommand, CommandCorrelationId>("Devices/commands/ping", command);
        }

        public Task<CommandCorrelationId> SendDiagnosticData(SendDiagnosticDataCommand command)
        {
            return _apiClient.Post<SendDiagnosticDataCommand, CommandCorrelationId>("Devices/commands/diagnostic-data", command);
        }
    }
}