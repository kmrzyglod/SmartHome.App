using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SmartHome.Application.Shared.Queries.General.GetDeviceList;
using SmartHome.Clients.WebApp.Services.Devices;
using SmartHome.Clients.WebApp.Services.Shared.NotificationsHub;

namespace SmartHome.Clients.WebApp.Pages.SystemModules.DevicesManager
{
    public class DevicesManagerView: ComponentBase, IDisposable
    {
        [Inject] protected IDevicesService DevicesService { get; set; } = null!;
        [Inject] protected INotificationsHub NotificationsHub { get; set; } = null!;
        protected IEnumerable<DeviceListEntryVm> Data { get; set; } = Enumerable.Empty<DeviceListEntryVm>();

        public async Task UpdateData()
        {
            var data = await DevicesService.GetDevicesList(new GetDeviceListQuery {PageNumber = 1, PageSize = 100});
            Data = data.Result;
        }

        protected override Task OnInitializedAsync()
        {
            return UpdateData();
        }

        public void Dispose()
        {
            
        }
    }
}
