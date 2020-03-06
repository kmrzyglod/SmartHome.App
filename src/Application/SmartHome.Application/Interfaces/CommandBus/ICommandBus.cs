using System.Threading.Tasks;
using SmartHome.Application.Interfaces.Command;
using SmartHome.Application.Models;

namespace SmartHome.Application.Interfaces.CommandBus
{
    public interface ICommandBus
    {
        Task<CommandCorrelationId> SendAsync<T>(T command) where T : ICommand;
    }
}