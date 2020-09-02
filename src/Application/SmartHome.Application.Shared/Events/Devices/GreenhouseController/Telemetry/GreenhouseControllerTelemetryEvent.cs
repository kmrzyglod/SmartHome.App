using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHome.Application.Shared.Events.Devices.GreenhouseController.Telemetry
{
    public class GreenhouseControllerTelemetryEvent
    {
        public DateTime MeasurementStartTime { get; }
        public DateTime MeasurementEndTime { get; }
        public float Temperature { get; }
        public float Pressure { get; }
        public float Humidity { get; }
        public int LightLevel { get; }
        public int SoilMoisture { get; }
        public float AverageWaterFlow { get; }
        public float MinWaterFlow { get; }
        public float MaxWaterFlow { get; }
        public float TotalWaterFlow { get; }
        public bool IsDoorOpen { get; }
    }
}
