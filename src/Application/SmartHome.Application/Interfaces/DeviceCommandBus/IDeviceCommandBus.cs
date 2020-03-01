using System.Threading.Tasks;
using SmartHome.Application.Interfaces.Command;

namespace SmartHome.Application.Interfaces.DeviceCommandBus
{
    public interface IDeviceCommandBus
    {
        Task SendCommandAsync<T>(T command) where T : IDeviceCommand;
    }
}