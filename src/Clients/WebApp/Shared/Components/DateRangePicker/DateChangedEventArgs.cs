using System;
using SmartHome.Application.Shared.Enums;

namespace SmartHome.Clients.WebApp.Shared.Components.DateRangePicker
{
    public class DateChangedEventArgs
    {
        public DateChangedEventArgs(DateTime? fromDate, DateTime? toDate, DateRangeGranulation granulation)
        {
            FromDate = fromDate;
            ToDate = toDate;
            Granulation = granulation;
        }

        public DateTime? FromDate { get; }
        public DateTime? ToDate { get; }
        public DateRangeGranulation Granulation { get; }
    }
}