using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;
using SmartHome.Application.Interfaces.Event;

namespace SmartHome.Infrastructure.EventBusMessageDeserializer
{
    public interface IEventGridMessageDeserializer
    {
        Task<IEvent> DeserializeAsync(EventGridEvent eventData);
    }
}