using System;
using System.Collections.Generic;
using System.Text;
using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Application.Shared.Events.Devices.GreenhouseController.Irrigation
{
    public class IrrigationFinishedEvent: EventBase
    {
        public double TotalWaterVolume { get; set; }
        public DateTime IrrigationStartTime { get; set;}
        public DateTime IrrigationEndTime { get; set;}
        public double AverageWaterFlow { get; set;}
        public double MinWaterFlow { get; set;}
        public double MaxWaterFlow { get; set;}
    }
}
