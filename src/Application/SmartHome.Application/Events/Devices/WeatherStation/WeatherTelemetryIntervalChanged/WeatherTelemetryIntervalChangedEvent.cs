using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Application.Events.Devices.WeatherStation.WeatherTelemetryIntervalChanged
{
    public class WeatherTelemetryIntervalChangedEvent : IEvent
    {
        public int NewInterval { get; set; }
        public string Source { get; set; } = string.Empty;
    }
}