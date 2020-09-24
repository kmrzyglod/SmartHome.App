using System;
using SmartHome.Application.Shared.Interfaces.Cache;

namespace SmartHome.Infrastructure.Cache
{
    public class CacheOptions : ICacheOptions
    {
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
    }
}