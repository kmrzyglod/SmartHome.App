using System;
using SmartHome.Domain.Enums;
using SmartHome.Domain.Interfaces;

namespace SmartHome.Domain.Entities.Devices.WeatherStation
{
    public class WeatherStationWindParameters : IMeasurement
    {
        public long Id { get; set; }
        public WindDirection MostFrequentWindDirection { get; set; }
        public WindDirection CurrentWindDirection { get; set; }
        public double MinWindSpeed { get; set; }
        public double MaxWindSpeed { get; set; }
        public double AverageWindSpeed { get; set; }
        public DateTime MeasurementStartTime { get; set; }
        public DateTime MeasurementEndTime { get; set; }
    }
}