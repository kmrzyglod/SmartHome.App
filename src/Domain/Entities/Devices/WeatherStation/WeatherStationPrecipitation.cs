using System;
using SmartHome.Domain.Interfaces;

namespace SmartHome.Domain.Entities.Devices.WeatherStation
{
    public class WeatherStationPrecipitation : IMeasurement
    {
        public long Id { get; set; }
        public DateTime MeasurementStartTime { get; set; }
        public DateTime MeasurementEndTime { get; set; }
        public double Rain { get; set; }
    }
}