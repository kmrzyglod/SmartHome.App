using System;

namespace SmartHome.Application.Shared.Queries.GreenhouseController.GetTemperatureAggregates
{
    public class TemperatureAggregatesVm
    {
        public double? MinTemperature { get; set; }
        public DateTime? MinTemperatureTimestamp { get; set; }
        public double? MaxTemperature { get; set; }
        public DateTime? MaxTemperatureTimestamp { get; set; }
    }
}