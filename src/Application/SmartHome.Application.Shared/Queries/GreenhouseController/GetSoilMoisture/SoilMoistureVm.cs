using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHome.Application.Shared.Queries.GreenhouseController.GetSoilMoisture
{
    public class SoilMoistureVm
    {
        public DateTime Timestamp { get; set; }
        public double SoilMoisture { get; set; }
        public int MaxSoilMoisture { get; set; }
        public int MinSoilMoisture { get; set; }
    }
}
