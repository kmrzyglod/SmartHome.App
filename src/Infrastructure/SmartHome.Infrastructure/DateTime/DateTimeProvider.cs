using System;
using SmartHome.Application.Interfaces.DateTime;

namespace SmartHome.Infrastructure.DateTime
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public System.DateTime GetUtcNow()
        {
            return System.DateTime.UtcNow;
        }
    }
}