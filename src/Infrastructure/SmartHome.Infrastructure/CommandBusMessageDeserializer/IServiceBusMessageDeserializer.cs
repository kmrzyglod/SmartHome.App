using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using SmartHome.Application.Shared.Interfaces.Command;

namespace SmartHome.Infrastructure.CommandBusMessageDeserializer
{
    public interface IServiceBusMessageDeserializer
    {
        Task<ICommand> DeserializeAsync(Message message);
    }
}