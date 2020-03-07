using System;

namespace SmartHome.Application.Shared.Enums
{
    //in minutes
    public enum DateRangeGranulation 
    {
        Raw = 1,
        FifteenMinutes = 900,
        HalfHour = 1800,
        Hour = 3600,
        ThreeHours = 10800,
        SixHours = 21600,
        Day = 86400,
        Month = 2592000,
        Year = 31622400
    }
}
