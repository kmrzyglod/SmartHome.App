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
            new GranulationType
            {
                Name = "Raw",
                Type = DateRangeGranulation.Raw
            },
            new GranulationType
            {
                Name = "15 minutes",
                Type = DateRangeGranulation.FifteenMinutes
            },
            new GranulationType
            {
                Name = "30 minutes",
                Type = DateRangeGranulation.HalfHour
            },
            new GranulationType
            {
                Name = "1 hour",
                Type = DateRangeGranulation.Hour
            },
            new GranulationType
            {
                Name = "3 hours",
                Type = DateRangeGranulation.ThreeHours
            },
            new GranulationType
            {
                Name = "6 hours",
                Type = DateRangeGranulation.SixHours
            },
            new GranulationType
            {
                Name = "Day",
                Type = DateRangeGranulation.Day
            },
            new GranulationType
            {
                Name = "Month",
                Type = DateRangeGranulation.Month
            },
            new GranulationType
            {
                Name = "Year",
                Type = DateRangeGranulation.Year
            }
        };
    }
}