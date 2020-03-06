using System;

namespace SmartHome.Application.Queries.Devices.Weather.GetPressure
{
    public class PressureVm
    {
        public DateTime Timestamp { get; set; }
        public double Pressure { get; set; }
    }
}
