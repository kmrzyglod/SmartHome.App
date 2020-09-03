using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHome.Application.Shared.Events.Devices.WindowsController.Telemetry
{
    public class WindowsControllerTelemetryEvent
    {
        public bool[] WindowsStatus { get; set; } = { };
    }
}
