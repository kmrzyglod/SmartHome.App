using SmartHome.Application.Shared.Interfaces.Command;

namespace SmartHome.Infrastructure.CommandBusMessageDeserializer
{
    public interface IServiceBusMessageDeserializer
    {
        ICommand DeserializeAsync(string message);
    }
}