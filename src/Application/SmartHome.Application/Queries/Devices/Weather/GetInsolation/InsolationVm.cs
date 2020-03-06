using System;

namespace SmartHome.Application.Queries.Devices.Weather.GetInsolation
{
    public class InsolationVm
    {
        public DateTime Timestamp { get; set; }
        public double LightLevel { get; set; }
    }
}
