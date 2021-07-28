using System;

namespace SmartHome.Clients.WebApp.Helpers
{
    public static class DateTimeExtensions
    {
        public static string ToDefaultFormat(this DateTime? date)
        {
            return $"{date?.ToLocalTime():dd.MM.yyyy HH:mm:ss}";
        }

        public static string ToDefaultFormat(this DateTime date)
        {
            return ToDefaultFormat(date as DateTime?);
        }
    }
}