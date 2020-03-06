using System;

namespace SmartHome.Application.Queries.Devices.Weather.GetTemperatureAggregates
{
    public class TemperatureAggregatesVm
    {
        public double? MinTemperature { get; set; }
        public DateTime? MinTemperatureTimestamp { get; set; }
        public double? MaxTemperature { get; set; }
        public DateTime? MaxTemperatureTimestamp { get; set; }
    }
}