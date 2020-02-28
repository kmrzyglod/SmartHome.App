using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using SmartHome.Application.Interfaces.Command;
using SmartHome.Infrastructure.Const;
using SmartHome.Infrastructure.EventBusMessageDeserializer;

namespace SmartHome.Infrastructure.CommandBusMessageDeserializer
{
    public class ServiceBusMessageDeserializer : IServiceBusMessageDeserializer
    {
        private readonly Assembly _commandTypesAssembly;

        public ServiceBusMessageDeserializer(Assembly commandTypesAssembly)
        {
            _commandTypesAssembly = commandTypesAssembly;
        }

        public async Task<ICommand> DeserializeAsync(Message message)
        {
            if (!message.UserProperties.TryGetValue(ParameterNames.ServiceBusMessageCommandNameParameter,
                out var commandNameObj))
            {
                throw new ServiceBusMessageDeserializationException(
                    $"No '{ParameterNames.ServiceBusMessageCommandNameParameter}' property found in message user properties");
            }

            if (!(commandNameObj is string commandName))
            {
                throw new ServiceBusMessageDeserializationException("Command name must be string");
            }

            if (message.Body == null)
            {
                throw new ServiceBusMessageDeserializationException("Message body (command) cannot be empty");
            }

            var commandType = _commandTypesAssembly.ExportedTypes.FirstOrDefault(x => x.Name == commandName);

            if (commandType == null)
            {
                throw new ServiceBusMessageDeserializationException(
                    $"Command: {commandName} is unsupported or wrong command types assembly was loaded");
            }

            if (!(await JsonSerializer.DeserializeAsync(
                new MemoryStream(message.Body),
                commandType) is ICommand deserializedCommand))
            {
                throw new EventGridMessageDeserializationException(
                    $"Cannot deserialize command: {commandName} because of invalid message body structure | Message body: {Encoding.UTF8.GetString(message.Body)}");
            }

            return deserializedCommand;
        }
    }
}