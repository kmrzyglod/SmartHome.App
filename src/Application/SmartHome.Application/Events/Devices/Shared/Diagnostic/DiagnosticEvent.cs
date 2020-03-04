using System;
using SmartHome.Application.Interfaces.Event;

namespace SmartHome.Application.Events.Devices.Shared.Diagnostic
{
    public class DiagnosticEvent : IEvent
    {
        public string? Ssid { get; set; }
        public double? Rssi { get; set; } //dB miliwats
        public string? Ip { get; set; }
        public string GatewayIp { get; set; }
        public uint? FreeHeapMemory { get; set; } //in bytes
        public string Source { get; set; } = string.Empty;
    }
}