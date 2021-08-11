using System.Threading.Tasks;
using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Application.Interfaces.EventBus
{
    public interface IEventBus
    {
        Task Publish<T>(T evt) where T : IEvent;
    }
}