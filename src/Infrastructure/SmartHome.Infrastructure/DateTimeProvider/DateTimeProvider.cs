using SmartHome.Application.Interfaces.DateTime;

namespace SmartHome.Infrastructure.DateTimeProvider
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public System.DateTime GetUtcNow()
        {
            return System.DateTime.UtcNow;
        }
    }
}