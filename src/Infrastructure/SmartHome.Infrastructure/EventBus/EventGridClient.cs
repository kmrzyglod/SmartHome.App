using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.EventGrid;
using SmartHome.Application.Interfaces.EventBus;
using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Infrastructure.EventBus
{
    public class EventGridClient : IEventBus
    {
        private readonly EventGridPublisherClient _client;

        public EventGridClient(EventGridPublisherClient client)
        {
            _client = client;
        }

        public async Task Publish<T>(T evt) where T : IEvent
        {
            string? serializedEvent = JsonSerializer.Serialize(evt);
            var eventGridData = new EventGridData
            {
                properties = new EventGridData.Properties
                {
                    MessageType = typeof(T).Name
                },
                body = Convert.ToBase64String(Encoding.UTF8.GetBytes(serializedEvent))
            };

            var result = await _client.SendEventAsync(new EventGridEvent(
                "App",
                "App",
                "1.0",
                eventGridData));

            if (result.Status >= 400)
            {
                throw new EventBusException(
                    $"Error during sending event to event bus. Status code: {result.Status} Message: {result.ReasonPhrase} Event body: {serializedEvent} ");
            }
        }
    }
}