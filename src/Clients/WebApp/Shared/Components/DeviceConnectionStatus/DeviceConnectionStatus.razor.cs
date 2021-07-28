using Microsoft.AspNetCore.Components;

namespace SmartHome.Clients.WebApp.Shared.Components.DeviceConnectionStatus
{
    public class DeviceConnectionStatusComponent: ComponentBase
    {
        [Parameter]
        public bool IsOnline { get; set; }
    }
}