using System;

namespace SmartHome.Application.Shared.Queries.SharedModels
{
    public class HumidityVm
    {
        public DateTime Timestamp { get; set; }
        public double Humidity { get; set; }
        public double MaxHumidity { get; set; }
        public double MinHumidity { get; set; }
    }
}
