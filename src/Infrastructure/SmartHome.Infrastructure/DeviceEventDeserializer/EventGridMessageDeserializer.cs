using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json.Linq;
using SmartHome.Application.Events.Devices.Shared;
using SmartHome.Application.Interfaces.Event;
using SmartHome.Infrastructure.Enums;
using SmartHome.Infrastructure.Helpers;

namespace SmartHome.Infrastructure.DeviceEventDeserializer
{
    public class EventGridMessageDeserializer : IEventGridMessageDeserializer
    {
        private static readonly Assembly _eventTypesAssembly = typeof(IEvent).Assembly;

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
            var message = (eventData.Data as JObject)?.ToObject<EventGridMessage>();

            if (message == null)
            {
                throw new EventGridMessageDeserializationException(
                    "Wrong message schema, cannot deserialize.");
            }

            if (message.Properties?.MessageType == null)
            {
                throw new EventGridMessageDeserializationException("Message type must set");
            }

            if (message.Body == null)
            {
                throw new EventGridMessageDeserializationException("Event payload cannot be null");
            }

            var eventType =
                _eventTypesAssembly.ExportedTypes.FirstOrDefault(x => x.Name == message.Properties.MessageType);

            if (eventType == null)
            {
                throw new EventGridMessageDeserializationException(
                    $"Event type: {message.Properties.MessageType} is unsupported");
            }

            var deserializedEvent = await JsonSerializer.DeserializeAsync(
                new MemoryStream(Convert.FromBase64String(message.Body)),
                eventType) as IEvent;

            if (deserializedEvent == null)
            {
                throw new EventGridMessageDeserializationException(
                    $"Cannot deserialize event: {message.Properties.MessageType}");
            }

            deserializedEvent.Source = eventData.Subject;

            return deserializedEvent;
        }
    }
}