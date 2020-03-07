using System;

namespace SmartHome.Application.Shared.Models
{
    public class CommandCorrelationId
    {
        public CommandCorrelationId(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get;  }
    }
}