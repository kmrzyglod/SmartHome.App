namespace SmartHome.Application.Shared.Events.Devices.WeatherStation.WeatherTelemetryIntervalChanged
{
    public class WeatherTelemetryIntervalChangedEvent : EventBase
    {
        public int NewInterval { get; set; }
    }
}