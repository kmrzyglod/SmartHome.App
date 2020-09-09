using System;

namespace SmartHome.Application.Shared.Queries.WeatherStation.GetPressure
{
    public class PressureVm
    {
        public DateTime Timestamp { get; set; }
        public double Pressure { get; set; }
        public double MaxPressure { get; set; }
        public double MinPressure { get; set; }
    }
}
