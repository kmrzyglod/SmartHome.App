using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SmartHome.Application.Shared.Events.Devices.Shared.DeviceConnected;
using SmartHome.Application.Shared.Events.Devices.Shared.DeviceCreated;
using SmartHome.Application.Shared.Events.Devices.Shared.DeviceDisconnected;
using SmartHome.Application.Shared.Interfaces.Event;
using SmartHome.Infrastructure.Enums;
using SmartHome.Infrastructure.Helpers;

namespace SmartHome.Infrastructure.EventBusMessageDeserializer
{
    public class EventGridMessageDeserializer : IEventGridMessageDeserializer
    {
        private readonly Assembly _eventTypesAssembly;

        public EventGridMessageDeserializer(Assembly eventTypesAssembly) => _eventTypesAssembly = eventTypesAssembly;

        public Task<IEvent> DeserializeAsync(EventGridEvent eventData)
        {
            return EnumHelpers.ToEnum<EventGridEventType>(eventData.EventType) switch
            {
                EventGridEventType.DeviceConnected => Task.FromResult(new DeviceConnectedEvent
                {
                    Source = eventData.Subject, Timestamp = eventData.EventTime
                } as IEvent),

                EventGridEventType.DeviceDisconnected => Task.FromResult(new DeviceDisconnectedEvent
                {
                    Source = eventData.Subject, Timestamp = eventData.EventTime
                } as IEvent),

                EventGridEventType.DeviceCreated => Task.FromResult(new DeviceCreatedEvent
                {
                    Source = eventData.Subject, Timestamp = eventData.EventTime
                } as IEvent),

                EventGridEventType.DeviceDeleted => Task.FromResult(new DeviceCreatedEvent
                {
                    Source = eventData.Subject, Timestamp = eventData.EventTime
                } as IEvent),

                EventGridEventType.DeviceTelemetry => DeserializeDefaultMessageAsync(eventData),

                EventGridEventType.App => DeserializeDefaultMessageAsync(eventData),
                EventGridEventType.Unknown => throw new EventGridMessageDeserializationException(
                    $"Unsupported event type: {eventData.EventType}"),
                _ => throw new EventGridMessageDeserializationException(
                    $"Unsupported event type: {eventData.EventType}")
            };
        }

        private async Task<IEvent> DeserializeDefaultMessageAsync(EventGridEvent eventData)
        {
            if (!eventData.Data.TryGetValue("properties", out var properties))
            {
                throw new EventGridMessageDeserializationException(
                    "Wrong message schema, cannot deserialize.");
            }

            var messageType = JObject.Parse(properties.ToString()).GetValue("MessageType")?.ToString();

            if (messageType == null)
            {
                throw new EventGridMessageDeserializationException("Message type must set");
            }

            if (!eventData.Data.TryGetValue("body", out var body))
            {
                throw new EventGridMessageDeserializationException(
                    "Wrong message schema, cannot deserialize.");
            }

            var messageBody = body.ToString();

            if (messageBody == null)
            {
                throw new EventGridMessageDeserializationException("Event payload cannot be null");
            }

            Type? eventType =
                _eventTypesAssembly.ExportedTypes.FirstOrDefault(x => x.Name == messageType);

            if (eventType == null)
            {
                throw new EventGridMessageDeserializationException(
                    $"Event type: {messageType} is unsupported or wrong event types assembly was loaded");
            }

            var deserializedEvent = await JsonSerializer.DeserializeAsync(
                new MemoryStream(Convert.FromBase64String(messageBody)),
                eventType) as IEvent;

            if (deserializedEvent == null)
            {
                throw new EventGridMessageDeserializationException(
                    $"Cannot deserialize event: {messageType} because of invalid message body structure | Message body: {messageBody}");
            }

            deserializedEvent.Source = eventData.Subject;

            return deserializedEvent;
        }
    }
}