using Microsoft.AspNetCore.Components;
using System;
using SmartHome.Application.Shared.Interfaces.DateTime;

namespace SmartHome.Clients.WebApp.Shared.Components.DateRangePicker
{
    public class DateRangePickerBase: ComponentBase
    {
        [Inject]
        protected IDateTimeProvider _dateTimeProvider {get; set; }

        [Parameter]
        public EventCallback<DateChnagedEventArgs> DateChanged { get; set; }
        [Parameter]
        public DateTime? FromDate { get; set; }
        [Parameter]
        public DateTime? ToDate { get; set; }

        protected void OnFromDateChanged(DateTime? fromDate)
        {
            FromDate = fromDate;
            DateChanged.InvokeAsync(new DateChnagedEventArgs(fromDate, ToDate));
        }

        protected void OnToDateChanged(DateTime? toDate)
        {
            ToDate = toDate;
            DateChanged.InvokeAsync(new DateChnagedEventArgs(FromDate?.AddDays(1), toDate));
        }

        protected override void OnInitialized()
        {
            FromDate = _dateTimeProvider.GetUtcNow().AddDays(-2).Date;
            ToDate = _dateTimeProvider.GetUtcNow().Date;
            base.OnInitialized();
        }
    }
}
