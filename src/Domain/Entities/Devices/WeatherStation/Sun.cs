using System;

namespace SmartHome.Domain.Entities.Devices.WeatherStation
{
    public class Sun
    {
        public DateTime MeasurementStartTime { get; set; }
        public DateTime MeasurementEndTime { get; set; }
        public double Insolation { get; set; }
    }
}