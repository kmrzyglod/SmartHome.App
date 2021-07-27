using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SmartHome.Application.Shared.Commands.Devices.Shared.Ping;
using SmartHome.Application.Shared.Queries.General.GetDeviceList;
using SmartHome.Application.Shared.Queries.General.GetDeviceStatus;
using SmartHome.Clients.WebApp.Services.Devices;
using SmartHome.Clients.WebApp.Services.Shared.CommandsExecutor;

namespace SmartHome.Clients.WebApp.Pages.SystemModules.DevicesManager
{
    public class DevicesManagerView : ComponentBase
    {
        [Inject] protected IDevicesService DevicesService { get; set; } = null!;
        [Inject] protected ICommandsExecutor CommandsExecutor { get; set; } = null!;

        protected IEnumerable<DeviceListRowVm> Data { get; set; } = Enumerable.Empty<DeviceListRowVm>();

        protected Task PingDevice(string deviceId)
        {
            return CommandsExecutor.ExecuteCommand((command) => DevicesService.Ping(command), new PingCommand
            {
                TargetDeviceId = deviceId
            }, timeoutInSeconds: 15);
        }

        public async Task UpdateData()
        {
            var data = await DevicesService.GetDevicesList(new GetDeviceListQuery {PageNumber = 1, PageSize = 100});
            var deviceDetailTasks = data.Result.Select(x =>
                DevicesService.GetDeviceStatus(new GetDeviceStatusQuery {DeviceId = x.DeviceId}));
            var deviceDetails = await Task.WhenAll(deviceDetailTasks);

            Data = data.Result.Zip(deviceDetails, (deviceData, deviceStatusDetails) => new DeviceListRowVm
            {
                DeviceId = deviceData.DeviceId,
                DeviceName = deviceData.DeviceName,
                IsOnline = deviceData.IsOnline,
                LastStatusUpdate = deviceData.LastStatusUpdate,
                DeviceStatusDetails = deviceStatusDetails
            });
        }

        protected override Task OnInitializedAsync()
        {
            return UpdateData();
        }
    }
}