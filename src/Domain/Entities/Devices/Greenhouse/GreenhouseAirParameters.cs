using System;
using SmartHome.Domain.Interfaces;

namespace SmartHome.Domain.Entities.Devices.Greenhouse
{
    public class GreenhouseAirParameters: IEntityWithTimestamp
    {
        public long Id { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
        public DateTime Timestamp { get; set; }
    }
}