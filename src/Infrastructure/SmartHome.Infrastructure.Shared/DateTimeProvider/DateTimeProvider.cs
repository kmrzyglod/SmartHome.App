using SmartHome.Application.Shared.Interfaces.DateTime;

namespace SmartHome.Infrastructure.Shared.DateTimeProvider
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public System.DateTime GetUtcNow()
        {
            return System.DateTime.UtcNow;
        }
    }
}