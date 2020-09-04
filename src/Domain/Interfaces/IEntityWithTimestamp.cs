using System;

namespace SmartHome.Domain.Interfaces
{
    public interface IEntityWithTimestamp
    {
        DateTime Timestamp { get; set; }
    }
}
