using System;

namespace SmartHome.Application.Shared.Queries.GetDeviceStatusHistory
{
    public class DeviceStatusHistoryVm
    {
        public string? Ssid { get; set; }
        public double? Rssi { get; set; } //dB miliwats
        public string? Ip { get; set; }
        public string? GatewayIp { get; set; }
        public uint? FreeHeapMemory { get; set; } //in bytes
        public DateTime Timestamp { get; set; }
        public string DeviceId { get; set; } = string.Empty;
    }
}
