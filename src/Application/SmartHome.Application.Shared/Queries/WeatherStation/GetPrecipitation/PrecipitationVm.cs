using System;

namespace SmartHome.Application.Shared.Queries.WeatherStation.GetPrecipitation
{
    public class PrecipitationVm
    {
        public double RainSum { get; set; }
        public DateTime Timestamp { get; set; }
    }
}