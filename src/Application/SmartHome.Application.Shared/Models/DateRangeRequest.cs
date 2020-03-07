using System;
using SmartHome.Application.Shared.Enums;

namespace SmartHome.Application.Shared.Models
{
    public class DateRangeRequest
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public DateRangeGranulation? Granulation { get; set; }

        public DateRangeRequest WithDefaultValues(DateTime dateFrom, DateTime dateTo, DateRangeGranulation granulation = DateRangeGranulation.Hour)
        {
            if (From == null)
            {
                From = dateFrom;
            }

            if (To == null)
            {
                To = dateTo;
            }

            if (Granulation == null)
            {
                Granulation = DateRangeGranulation.Hour;
            }

            return this;
        }
    }
}