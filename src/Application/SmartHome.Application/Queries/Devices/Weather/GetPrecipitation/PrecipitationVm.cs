using System;

namespace SmartHome.Application.Queries.Devices.Weather.GetPrecipitation
{
    public class PrecipitationVm
    {
        public double RainSum { get; set; }
        public DateTime Timestamp { get; set; }
    }
}