﻿using System.Threading.Tasks;
using SmartHome.Application.Shared.Interfaces.Event;

namespace SmartHome.Infrastructure.EventBusMessageDeserializer
{
    public interface IEventGridMessageDeserializer
    {
        Task<IEvent> DeserializeAsync(EventGridEvent eventData);
    }
}