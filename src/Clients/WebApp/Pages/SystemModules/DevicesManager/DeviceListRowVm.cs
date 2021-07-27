using SmartHome.Application.Shared.Queries.General.GetDeviceList;
using SmartHome.Application.Shared.Queries.General.GetDeviceStatus;

namespace SmartHome.Clients.WebApp.Pages.SystemModules.DevicesManager
{
    public class DeviceListRowVm: DeviceListEntryVm
    {
        public DeviceStatusVm DeviceStatusDetails { get; set; }
    }
}
