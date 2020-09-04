using System;

namespace SmartHome.Application.Shared.Events.Devices.GreenhouseController.Telemetry
{
    public class GreenhouseControllerTelemetryEvent: EventBase
    {
        public DateTime MeasurementStartTime { get; set; }
        public DateTime MeasurementEndTime { get; set; }
        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
        public int LightLevel { get; set; }
        public int SoilMoisture { get; set; }
        public bool IsDoorOpen { get; set; }
    }
}
