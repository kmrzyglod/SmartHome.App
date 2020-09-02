using System.Threading.Tasks;
using SmartHome.Application.Shared.Interfaces.Command;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Interfaces.CommandBus
{
    public interface ICommandBus
    {
        Task<CommandCorrelationId> SendAsync<T>(T command) where T : ICommand;
    }
}