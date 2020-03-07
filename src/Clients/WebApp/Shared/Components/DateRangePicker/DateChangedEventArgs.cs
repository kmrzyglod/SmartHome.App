using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Clients.WebApp.Shared.Components.DateRangePicker
{
    public class DateChnagedEventArgs
    {
        public DateChnagedEventArgs(DateTime? fromDate, DateTime? toDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
        }

        public DateTime? FromDate { get; }
        public DateTime? ToDate { get; }
    }
}
