using System.Collections.Generic;
using SmartHome.Application.Shared.Enums;

namespace SmartHome.Clients.WebApp.Shared.Components.DateRangePicker
{
    public class GranulationType
    {
        public string Name { get; set; } = string.Empty;
        public DateRangeGranulation Type { get; set; }

        public static IEnumerable<GranulationType> Types { get; } = new List<GranulationType>
        {
            new()
            {
                Name = "Raw",
                Type = DateRangeGranulation.Raw
            },
            new()
            {
                Name = "15 minutes",
                Type = DateRangeGranulation.FifteenMinutes
            },
            new()
            {
                Name = "30 minutes",
                Type = DateRangeGranulation.HalfHour
            },
            new()
            {
                Name = "1 hour",
                Type = DateRangeGranulation.Hour
            },
            new()
            {
                Name = "3 hours",
                Type = DateRangeGranulation.ThreeHours
            },
            new()
            {
                Name = "6 hours",
                Type = DateRangeGranulation.SixHours
            },
            new()
            {
                Name = "Day",
                Type = DateRangeGranulation.Day
            },
            new()
            {
                Name = "Month",
                Type = DateRangeGranulation.Month
            },
            new()
            {
                Name = "Year",
                Type = DateRangeGranulation.Year
            }
        };
    }
}