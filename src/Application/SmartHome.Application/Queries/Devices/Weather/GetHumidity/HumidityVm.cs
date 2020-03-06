using System;

namespace SmartHome.Application.Queries.Devices.Weather.GetHumidity
{
    public class HumidityVm
    {
        public DateTime Timestamp { get; set; }
        public double Humidity { get; set; }
    }
}
