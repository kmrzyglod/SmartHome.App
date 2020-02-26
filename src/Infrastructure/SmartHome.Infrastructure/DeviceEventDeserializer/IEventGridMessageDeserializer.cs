using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.EventHubs;
using SmartHome.Application.Interfaces.Event;

namespace SmartHome.Infrastructure.DeviceEventDeserializer
{
    public interface IEventGridMessageDeserializer
    {
        Task<IEvent> DeserializeAsync(EventGridEvent eventData);
    }
}