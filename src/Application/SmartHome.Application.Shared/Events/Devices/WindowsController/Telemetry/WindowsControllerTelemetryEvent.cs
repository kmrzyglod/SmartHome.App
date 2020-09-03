using System;
using System.Collections.Generic;
using System.Text;
using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Application.Shared.Events.Devices.WindowsController.Telemetry
{
    public class WindowsControllerTelemetryEvent: IEvent
    {
        public bool[] WindowsStatus { get; set; } = { };
        public string Source { get; set; }  = string.Empty;
    }
}
