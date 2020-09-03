﻿using System;
using System.Collections.Generic;
using System.Text;
using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Application.Shared.Events.Devices.GreenhouseController.Telemetry
{
    public class GreenhouseControllerTelemetryEvent: IEvent
    {
        public DateTime MeasurementStartTime { get; set; }
        public DateTime MeasurementEndTime { get; set; }
        public float Temperature { get; set; }
        public float Pressure { get; set; }
        public float Humidity { get; set; }
        public int LightLevel { get; set; }
        public int SoilMoisture { get; set; }
        public float AverageWaterFlow { get; set; }
        public float MinWaterFlow { get; set; }
        public float MaxWaterFlow { get; set; }
        public float TotalWaterFlow { get; set; }
        public bool IsDoorOpen { get; set; }
        public string Source { get; set; } = string.Empty;
    }
}