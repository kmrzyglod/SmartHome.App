using System;
using SmartHome.Domain.Enums;

namespace SmartHome.Application.Shared.Events.Devices.WeatherStation.Telemetry
{
    public class WeatherTelemetryEvent : EventBase
    {
        public DateTime MeasurementStartTime { get; set; }
        public DateTime MeasurementEndTime { get; set; }
        public double Temperature { get; set; } // [Celsius] 
        public double Pressure { get; set; } // [hPa]
        public double Humidity { get; set; } // [%]
        public int LightLevel { get; set; } // [lux]
        public WindDirection CurrentWindDirection { get; set; }
        public WindDirection MostFrequentWindDirection { get; set; }
        public double AverageWindSpeed { get; set; } // [m/s]
        public double MaxWindSpeed { get; set; } // [m/s]
        public double MinWindSpeed { get; set; } // [m/s]
        public double Precipitation { get; set; } // [mm]
    }
}