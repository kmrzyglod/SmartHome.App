using System;
using SmartHome.Domain.Interfaces;

namespace SmartHome.Domain.Entities.Devices.Greenhouse
{
    public class GreenhouseIrrigationHistory: IMeasurement
    {
        public long Id { get; set; }        
        public float TotalWaterVolume { get; set;}
        public float AverageWaterFlow { get; set;}
        public float MinWaterFlow { get; set;}
        public float MaxWaterFlow { get; set;}
        public DateTime MeasurementStartTime { get; set; }
        public DateTime MeasurementEndTime { get; set; }
    }
}
