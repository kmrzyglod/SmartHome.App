using System;

namespace SmartHome.Application.Shared.Queries.General.GetDeviceStatus
{
    public class DeviceStatusVm
    {
        public string DeviceName { get; set; } = string.Empty;
        public bool IsOnline { get; set; }
        public string? Ssid { get; set; }
        public double? Rssi { get; set; } //dB miliwats
        public string? Ip { get; set; }
        public string? GatewayIp { get; set; }
        public uint? FreeHeapMemory { get; set; } //in bytes
        public DateTime LastStatusUpdate { get; set; }
    }
}