using System;

namespace SmartHome.Application.Interfaces.Message
{
    public interface IMessage
    {
        Guid CorrelationId { get; set; }
    }
}
