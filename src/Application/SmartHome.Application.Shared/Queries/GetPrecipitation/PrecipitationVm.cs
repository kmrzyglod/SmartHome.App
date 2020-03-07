using System;

namespace SmartHome.Application.Shared.Queries.GetPrecipitation
{
    public class PrecipitationVm
    {
        public double RainSum { get; set; }
        public DateTime Timestamp { get; set; }
    }
}