namespace SmartHome.Application.Shared.Events.Devices.WindowsController.Telemetry
{
    public class WindowsControllerTelemetryEvent: EventBase
    {
        public bool[] WindowsStatus { get; set; } = { };
    }
}
