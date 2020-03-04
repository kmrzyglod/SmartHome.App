#pragma warning disable CS8618 
namespace SmartHome.Domain.Entities.Devices.Shared
{
    public class DeviceStatus
    {
        public long Id { get; set; }
        public string? Ssid { get; set; }
        public double? Rssi { get; set; } //dB miliwats
        public string? Ip { get; set; }
        public string? GatewayIp { get; set; }
        public uint? FreeHeapMemory { get; set; } //in bytes
        public Device Device { get; set; } 
    }
}