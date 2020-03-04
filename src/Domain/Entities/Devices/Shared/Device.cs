using System;

namespace SmartHome.Domain.Entities.Devices.Shared
{
    public class Device
    {
        public long Id { get; set; }
        public string DeviceId { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public bool IsOnline { get; set; }
        public DateTime LastStatusUpdate { get; set; }
        public bool IsDeleted { get; set; }

        public void Offline()
        {
            IsOnline = false;
        }

        public void Online()
        {
            IsOnline = true;
        }
    }
}
