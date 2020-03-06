using System;

namespace SmartHome.Application.Queries.Devices.Weather.GetTemperature
{
    public class TemperatureVm
    {
        public double Temperature { get; set; }
        public DateTime Timestamp { get; set; }
    }
}