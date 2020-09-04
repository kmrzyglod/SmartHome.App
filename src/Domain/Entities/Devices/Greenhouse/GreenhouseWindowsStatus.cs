using System;
using SmartHome.Domain.Interfaces;

namespace SmartHome.Domain.Entities.Devices.Greenhouse
{
    public class GreenhouseWindowsStatus: IEntityWithTimestamp
    {
        public long Id { get; set; }
        public bool Window1Opened { get; set; }
        public bool Window2Opened { get; set; }
        public bool DoorOpened { get; set; }
        public DateTime Timestamp { get; set; }
    }
}