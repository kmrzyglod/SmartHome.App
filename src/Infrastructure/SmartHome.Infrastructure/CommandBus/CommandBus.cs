using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using SmartHome.Application.Interfaces.Command;
using SmartHome.Application.Interfaces.CommandBus;
using SmartHome.Infrastructure.Const;

namespace SmartHome.Infrastructure.CommandBus
{
    public class CommandBus : ICommandBus
    {
        private readonly IQueueClient _queueClient;

        public CommandBus(IQueueClient queueClient)
        {
            _queueClient = queueClient;
        }

        public async Task<Guid> SendAsync<T>(T command) where T: ICommand
        {
            var correlationId = Guid.NewGuid();
            command.CorrelationId = correlationId;

            var message = new Message(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(command)))
            {
                CorrelationId = correlationId.ToString()
            };

            message.UserProperties.Add(ParameterNames.ServiceBusMessageCommandNameParameter, command.GetType().Name);

            await _queueClient.SendAsync(message);
            return correlationId;
        }
    }
}