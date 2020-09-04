using System;
using System.Collections.Generic;
using System.Text;
using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Application.Shared.Events.Devices.GreenhouseController.Irrigation
{
    public class IrrigationFinishedEvent: EventBase
    {
        public float TotalWaterVolume { get; set; }
        public DateTime IrrigationStartTime { get; set;}
        public DateTime IrrigationEndTime { get; set;}
        public float AverageWaterFlow { get; set;}
        public float MinWaterFlow { get; set;}
        public float MaxWaterFlow { get; set;}
    }
}
