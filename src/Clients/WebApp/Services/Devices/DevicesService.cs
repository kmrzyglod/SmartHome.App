using System.Threading.Tasks;
using SmartHome.Application.Shared.Commands.Devices.GreenhouseController.Irrigation;
using SmartHome.Application.Shared.Commands.Devices.Shared.Ping;
using SmartHome.Application.Shared.Commands.Devices.Shared.SendDiagnosticData;
using SmartHome.Application.Shared.Commands.Devices.WindowsController.CloseWindow;
using SmartHome.Application.Shared.Commands.Devices.WindowsController.OpenWindow;
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

        public Task<CommandCorrelationId> OpenWindow(OpenWindowCommand command)
        {
            return _apiClient.Post<OpenWindowCommand, CommandCorrelationId>("Devices/window-manager/commands/open-window", command);
        }

        public Task<CommandCorrelationId> CloseWindow(CloseWindowCommand command)
        {
            return _apiClient.Post<CloseWindowCommand, CommandCorrelationId>("Devices/window-manager/commands/close-window", command);
        }

        public Task<CommandCorrelationId> Irrigate(IrrigateCommand command)
        {
            return _apiClient.Post<IrrigateCommand, CommandCorrelationId>("Devices/greenhouse-controller/commands/irrigate", command);
        }

        public Task<CommandCorrelationId> AbortIrrigation(AbortIrrigationCommand command)
        {
            return _apiClient.Post<AbortIrrigationCommand, CommandCorrelationId>("Devices/greenhouse-controller/commands/abort-irrigation", command);
        }
    }
}