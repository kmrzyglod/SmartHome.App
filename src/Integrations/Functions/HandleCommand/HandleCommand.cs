using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SmartHome.Infrastructure.CommandBusMessageDeserializer;

namespace SmartHome.Integrations.Functions.HandleCommand
{
    public class HandleCommand
    {
        private readonly IServiceBusMessageDeserializer _commandBusMessageDeserializer;
        private readonly IMediator _mediator;

        public HandleCommand(IServiceBusMessageDeserializer commandBusMessageDeserializer, IMediator mediator)
        {
            _commandBusMessageDeserializer = commandBusMessageDeserializer;
            _mediator = mediator;
        }

        [FunctionName("HandleCommand")]
        public async Task Run([ServiceBusTrigger("commands", Connection = "ServiceBusConnectionString")]
            Message message, ILogger log)
        {
            var command = await _commandBusMessageDeserializer.DeserializeAsync(message);
            await _mediator.Send(command);
        }
    }
}