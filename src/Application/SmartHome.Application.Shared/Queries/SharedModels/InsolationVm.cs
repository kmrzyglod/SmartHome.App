using System;

namespace SmartHome.Application.Shared.Queries.SharedModels
{
    public class InsolationVm
    {
        public DateTime Timestamp { get; set; }
        public double LightLevel { get; set; }
        public double MaxLightLevel { get; set; }
        public double MinLightLevel { get; set; }
    }
}
