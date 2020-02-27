using System.Runtime.Serialization;

namespace SmartHome.Infrastructure.Enums
{
    enum EventGridEventType
    {
        Unknown, 
        [EnumMember(Value = "App")]
        App, 
        [EnumMember(Value = "Microsoft.Devices.DeviceTelemetry")]
        DeviceTelemetry,
        [EnumMember(Value = "Microsoft.Devices.DeviceConnected")]
        DeviceConnected,
        [EnumMember(Value = "Microsoft.Devices.DeviceDisconnected")]
        DeviceDisconnected,
        [EnumMember(Value = "Microsoft.Devices.DeviceCreated")]
        DeviceCreated,
        [EnumMember(Value = "Microsoft.Devices.DeviceDeleted")]
        DeviceDeleted
    }
}
