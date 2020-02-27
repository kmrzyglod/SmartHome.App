using System;
using SmartHome.Domain.Interfaces;

namespace SmartHome.Domain.Entities.Devices.WeatherStation
{
    public class Air : IMeasurement
    {
        public long Id { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
        public DateTime MeasurementStartTime { get; set; }
        public DateTime MeasurementEndTime { get; set; }
    }
}