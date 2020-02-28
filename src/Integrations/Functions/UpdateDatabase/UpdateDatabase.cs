// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using SmartHome.Infrastructure.EventBusMessageDeserializer;

namespace SmartHome.Integrations.Functions.UpdateDatabase
{
    public class UpdateDatabase
    {
        private readonly IEventGridMessageDeserializer _deserializer;
        private readonly IMediator _mediator;

        public UpdateDatabase(IMediator mediator, IEventGridMessageDeserializer deserializer)
        {
            _mediator = mediator;
            _deserializer = deserializer;
        }

        [FunctionName("UpdateDatabase")]
        public async Task Run([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            var @event = await _deserializer.DeserializeAsync(eventGridEvent);
            await _mediator.Publish(@event);
        }
    }
}