using System;
using SmartHome.Domain.Interfaces;

namespace SmartHome.Domain.Entities.Devices.Greenhouse
{
    public class GreenhouseInsolationParameters: IEntityWithTimestamp
    {
        public long Id { get; set; }
        public double LightLevelInLux { get; set; }
        public DateTime Timestamp { get; set; }
    }
}