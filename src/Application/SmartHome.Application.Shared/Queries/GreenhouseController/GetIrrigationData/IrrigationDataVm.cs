using System;

namespace SmartHome.Application.Shared.Queries.GreenhouseController.GetIrrigationData
{
    public class IrrigationDataVm
    {
        public DateTime Timestamp { get; set; }
        public double TotalWaterVolume { get; set; }
        public double AverageWaterFlow { get; set;}
        public double MinWaterFlow { get; set;}
        public double MaxWaterFlow { get; set;}
    }
}
