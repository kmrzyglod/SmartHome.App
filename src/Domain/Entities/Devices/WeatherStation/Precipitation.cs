using System;

namespace SmartHome.Domain.Entities.Devices.WeatherStation
{
    public class Precipitation
    {
        public DateTime MeasurementStartTime { get; set; }
        public DateTime MeasurementEndTime { get; set; }
        public double Rain { get; set; }
    }
}