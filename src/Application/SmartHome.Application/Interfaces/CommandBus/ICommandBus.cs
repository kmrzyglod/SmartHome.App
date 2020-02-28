using System;
using System.Threading.Tasks;
using SmartHome.Application.Interfaces.Command;

namespace SmartHome.Application.Interfaces.CommandBus
{
    public interface ICommandBus
    {
        Task<Guid> SendAsync(ICommand command);
    }
}