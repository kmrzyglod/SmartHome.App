﻿using System;

namespace SmartHome.Application.Shared.Queries.SharedModels
{
    public class TemperatureVm
    {
        public double Temperature { get; set; }
        public double MinTemperature { get; set; }
        public double MaxTemperature { get; set; }
        public DateTime Timestamp { get; set; }
    }
}