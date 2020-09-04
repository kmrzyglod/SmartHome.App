using System;

namespace SmartHome.Application.Shared.Events.Devices.GreenhouseController.Telemetry
{
    public class GreenhouseControllerTelemetryEvent: EventBase
    {
        public DateTime MeasurementStartTime { get; set; }
        public DateTime MeasurementEndTime { get; set; }
        public float Temperature { get; set; }
        public float Pressure { get; set; }
        public float Humidity { get; set; }
        public int LightLevel { get; set; }
        public int SoilMoisture { get; set; }
        public bool IsDoorOpen { get; set; }
    }
}
