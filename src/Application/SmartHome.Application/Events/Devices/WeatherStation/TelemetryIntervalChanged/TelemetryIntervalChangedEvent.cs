using SmartHome.Application.Interfaces.Event;

namespace SmartHome.Application.Events.Devices.WeatherStation.TelemetryIntervalChanged
{
    public class TelemetryIntervalChangedEvent : IEvent
    {
        public int NewInterval { get; set; }
        public string Source { get; set; }
    }
}