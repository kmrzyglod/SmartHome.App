﻿using MongoDB.Bson.Serialization;
using SmartHome.Application.Shared.Events;
using SmartHome.Application.Shared.Events.App;
using SmartHome.Application.Shared.Events.Devices.GreenhouseController.Door;
using SmartHome.Application.Shared.Events.Devices.GreenhouseController.Irrigation;
using SmartHome.Application.Shared.Events.Devices.GreenhouseController.Telemetry;
using SmartHome.Application.Shared.Events.Devices.Shared.DeviceConnected;
using SmartHome.Application.Shared.Events.Devices.Shared.DeviceCreated;
using SmartHome.Application.Shared.Events.Devices.Shared.DeviceDeleted;
using SmartHome.Application.Shared.Events.Devices.Shared.DeviceDisconnected;
using SmartHome.Application.Shared.Events.Devices.Shared.DeviceStatusUpdated;
using SmartHome.Application.Shared.Events.Devices.Shared.Diagnostic;
using SmartHome.Application.Shared.Events.Devices.Shared.Error;
using SmartHome.Application.Shared.Events.Devices.WeatherStation.Telemetry;
using SmartHome.Application.Shared.Events.Devices.WeatherStation.WeatherTelemetryIntervalChanged;
using SmartHome.Application.Shared.Events.Devices.WindowsController.Telemetry;
using SmartHome.Application.Shared.Events.Devices.WindowsController.WindowClosed;
using SmartHome.Application.Shared.Events.Devices.WindowsController.WindowOpened;

namespace SmartHome.Infrastructure.EventStore
{
    //TODO do this dynamically using reflection (register all IEvent derived classes) to avoid issues with unregistered event types
    public static class MongoClassMap
    {
        //old types

        public static void RegisterClassMap()
        {
            
            //General
            BsonClassMap.RegisterClassMap<DiagnosticEvent>();
            BsonClassMap.RegisterClassMap<DeviceConnectedEvent>();
            BsonClassMap.RegisterClassMap<DeviceCreatedEvent>();
            BsonClassMap.RegisterClassMap<DeviceDeletedEvent>();
            BsonClassMap.RegisterClassMap<DeviceDisconnectedEvent>();
            BsonClassMap.RegisterClassMap<DeviceStatusUpdatedEvent>();
            BsonClassMap.RegisterClassMap<CommandResultEvent>();
            BsonClassMap.RegisterClassMap<ErrorEvent>();
           
            //Weather station
            BsonClassMap.RegisterClassMap<WeatherTelemetryEvent>();
            BsonClassMap.RegisterClassMap<WeatherTelemetryIntervalChangedEvent>();
            
            //Greenhouse controller
            BsonClassMap.RegisterClassMap<GreenhouseControllerTelemetryEvent>();
            BsonClassMap.RegisterClassMap<DoorClosedEvent>();
            BsonClassMap.RegisterClassMap<DoorOpenedEvent>();
            BsonClassMap.RegisterClassMap<IrrigationFinishedEvent>();
            BsonClassMap.RegisterClassMap<RuleExecutionResultEvent>();
           
            //Windows controller
            BsonClassMap.RegisterClassMap<WindowClosedEvent>();
            BsonClassMap.RegisterClassMap<WindowOpenedEvent>();
            BsonClassMap.RegisterClassMap<WindowsControllerTelemetryEvent>();
        }
    }
}
