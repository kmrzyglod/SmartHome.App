using System;

namespace SmartHome.Application.Shared.Interfaces.Cache
{
    public interface ICacheOptions
    {
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
    }
}