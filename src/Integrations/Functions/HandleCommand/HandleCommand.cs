using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.Functions.Worker;
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

        [Function("HandleCommand")]
        public async Task Run([ServiceBusTrigger("commands", Connection = "ServiceBusConnectionString")]
            string message, FunctionContext context)
        {
            var command = _commandBusMessageDeserializer.DeserializeAsync(message);
            await _mediator.Send(command);
        }
    }
}