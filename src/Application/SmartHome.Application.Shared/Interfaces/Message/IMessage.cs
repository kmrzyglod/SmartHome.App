using System;

namespace SmartHome.Application.Shared.Interfaces.Message
{
    public interface IMessage
    {
        Guid CorrelationId { get; set; }
    }
}
