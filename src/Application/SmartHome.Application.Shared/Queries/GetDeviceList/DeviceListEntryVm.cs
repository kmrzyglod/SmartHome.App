using System;

namespace SmartHome.Application.Shared.Queries.GetDeviceList
{
    public class DeviceListEntryVm
    {
        public string DeviceId { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public bool IsOnline { get; set; }
        public DateTime LastStatusUpdate { get; set; }
    }
}
