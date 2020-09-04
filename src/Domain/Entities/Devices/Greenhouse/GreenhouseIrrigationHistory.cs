using System;
using SmartHome.Domain.Interfaces;

namespace SmartHome.Domain.Entities.Devices.Greenhouse
{
    public class GreenhouseIrrigationHistory: IMeasurement
    {
        public long Id { get; set; }        
        public double TotalWaterVolume { get; set;}
        public double AverageWaterFlow { get; set;}
        public double MinWaterFlow { get; set;}
        public double MaxWaterFlow { get; set;}
        public DateTime MeasurementStartTime { get; set; }
        public DateTime MeasurementEndTime { get; set; }
    }
}
