using System;
using SmartHome.Domain.Interfaces;

namespace SmartHome.Domain.Entities.Devices.Greenhouse
{
    public class GreenhouseSoilParameters: IEntityWithTimestamp
    {
        public long Id { get; set; }
        public int SoilMoisture { get; set; }
        public DateTime Timestamp { get; set; }
    }
}