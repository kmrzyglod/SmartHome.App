using MongoDB.Bson.Serialization;
using SmartHome.Application.Events;
using SmartHome.Application.Events.Devices.Shared.DeviceConnected;
using SmartHome.Application.Events.Devices.Shared.DeviceCreated;
using SmartHome.Application.Events.Devices.Shared.DeviceDeleted;
using SmartHome.Application.Events.Devices.Shared.DeviceDisconnected;
using SmartHome.Application.Events.Devices.Shared.DeviceStatusUpdated;
using SmartHome.Application.Events.Devices.Shared.Diagnostic;
using SmartHome.Application.Events.Devices.Shared.Error;
using SmartHome.Application.Events.Devices.WeatherStation.Telemetry;
using SmartHome.Application.Events.Devices.WeatherStation.WeatherTelemetryIntervalChanged;

namespace SmartHome.Infrastructure.EventStore
{
    //TODO do this dynamically using reflection (register all IEvent derived classes) to avoid issues with unregistered event types
    public static class MongoClassMap
    {
        //old types

        public class TelemetryEvent: WeatherTelemetryEvent { }

        public static void RegisterClassMap()
        {
            BsonClassMap.RegisterClassMap<WeatherTelemetryEvent>();
            BsonClassMap.RegisterClassMap<DiagnosticEvent>();
            BsonClassMap.RegisterClassMap<DeviceConnectedEvent>();
            BsonClassMap.RegisterClassMap<DeviceCreatedEvent>();
            BsonClassMap.RegisterClassMap<DeviceDeletedEvent>();
            BsonClassMap.RegisterClassMap<DeviceDisconnectedEvent>();
            BsonClassMap.RegisterClassMap<DeviceStatusUpdatedEvent>();
            BsonClassMap.RegisterClassMap<CommandResultEvent>();
            BsonClassMap.RegisterClassMap<ErrorEvent>();
            BsonClassMap.RegisterClassMap<WeatherTelemetryIntervalChangedEvent>();
            
            //Old eventTypes
            BsonClassMap.RegisterClassMap<TelemetryEvent>();
        }
    }
}
