using System;

namespace SmartHome.Application.Shared.Queries.GreenhouseController.GetWindowsStatus
{
    public class WindowsStatusVm
    {
        public DateTime DoorLastStatusUpdate { get; set; }
        public DateTime WindowsLastStatusUpdate { get; set; }
        public bool Window1 { get; set; }
        public bool Window2 { get; set; }
        public bool Door { get; set; }
    }
}