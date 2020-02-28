using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using SmartHome.Application.Interfaces.Command;

namespace SmartHome.Infrastructure.CommandBusMessageDeserializer
{
    public interface IServiceBusMessageDeserializer
    {
        Task<ICommand> DeserializeAsync(Message message);
    }
}