using System;

namespace SmartHome.Application.Models
{
    public class DateRangeRequest
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public DateRangeRequest WithDefaultValues(DateTime dateFrom, DateTime dateTo)
        {
            if (From == null)
            {
                From = dateFrom;
            }

            if (To == null)
            {
                To = dateTo;
            }

            return this;
        }
    }
}