using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json.Linq;
using SmartHome.Application.Shared.Interfaces.Command;
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

        public ICommand DeserializeAsync(string message)
        {
            if (message == null)
            {
                throw new ServiceBusMessageDeserializationException("Message body (command) cannot be empty");
            }

            if (!JObject.Parse(message).TryGetValue(ParameterNames.ServiceBusMessageCommandNameParameter,
                out var commandNameObj))
            {
                throw new ServiceBusMessageDeserializationException(
                    $"No '{ParameterNames.ServiceBusMessageCommandNameParameter}' property found in message user properties");
            }

            var commandName = commandNameObj.ToString();
            var commandType = _commandTypesAssembly.ExportedTypes.FirstOrDefault(x => x.Name == commandName);

            if (commandType == null)
            {
                throw new ServiceBusMessageDeserializationException(
                    $"Command: {commandName} is unsupported or wrong command types assembly was loaded");
            }

            if (!(JsonSerializer.Deserialize(
                message,
                commandType) is ICommand deserializedCommand))
            {
                throw new EventGridMessageDeserializationException(
                    $"Cannot deserialize command: {commandName} because of invalid message body structure | Message body: {message}");
            }

            return deserializedCommand;
        }
    }
}