using System;
using SmartHome.Application.Interfaces.Event;
using SmartHome.Domain.Enums;

namespace SmartHome.Application.Events.Devices.WeatherStation.Telemetry
{
    public class TelemetryEvent : IEvent
    {
        public DateTime MeasurementStartTime { get; set; }
        public DateTime MeasurementEndTime { get; set; }
        public double Temperature { get; set; }// [Celsius] 
        public double Pressure { get; set; } // [hPa]
        public double Humidity{ get; set; } // [%]
        public int LightLevel { get; set; } // [lux]
        public WindDirection CurrentWindDirection { get; set; }
        public WindDirection MostFrequentWindDirection { get; set; }
        public float AverageWindSpeed { get; set; } // [m/s]
        public float MaxWindSpeed { get; set; }// [m/s]
        public float MinWindSpeed { get; set; } // [m/s]
        public float Precipitation { get; set; } // [mm]
        public string Source { get; set; } = string.Empty;
    }
}