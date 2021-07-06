using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using SmartHome.Application.Shared.Enums;

namespace SmartHome.Clients.WebApp.Shared.Components.DateRangePicker
{
    public class DateRangePickerBase : ComponentBase
    {
        private GranulationType _selectedGranulationType =
            GranulationType.Types.First(x => x.Type == DateRangeGranulation.Hour);

        protected IEnumerable<GranulationType> GranulationTypes = GranulationType.Types;
        protected DateTime MaxDateToDate { get; set; }
        protected DateTime MaxDateFromDate { get; set; }
        protected DateTime MinDateToDate { get; set; }

        public GranulationType SelectedGranulationType
        {
            get => _selectedGranulationType;
            protected set
            {
                _selectedGranulationType = value;
                DateChanged.InvokeAsync(new DateChangedEventArgs(FromDate, ToDate, value.Type));
            }
        }

        [Parameter] public EventCallback<DateChangedEventArgs> DateChanged { get; set; }

        [Parameter] public DateTime DefaultFromDate { get; set; }

        [Parameter] public DateTime DefaultToDate { get; set; }

        public DateTime FromDate { get; protected set; }

        public DateTime ToDate { get; protected set; }

        protected void OnFromDateChanged(DateTime fromDate)
        {
            if (fromDate > ToDate)
            {
                return;
            }

            FromDate = fromDate;
            DateChanged.InvokeAsync(new DateChangedEventArgs(fromDate, ToDate, SelectedGranulationType.Type));
            CalculateDateRanges();
        }

        protected void OnToDateChanged(DateTime toDate)
        {
            if (toDate < FromDate)
            {
                return;
            }

            ToDate = toDate;
            DateChanged.InvokeAsync(new DateChangedEventArgs(FromDate.AddDays(1), toDate,
                SelectedGranulationType.Type));
            CalculateDateRanges();
        }

        private void CalculateDateRanges()
        {
            var currentDateTime = DateTime.Now;
            MaxDateToDate = currentDateTime.AddDays(1);
            MaxDateFromDate = ToDate.AddDays(-1);
            MinDateToDate = FromDate.AddDays(1);
        }

        protected override void OnInitialized()
        {
            FromDate = DefaultFromDate;
            ToDate = DefaultToDate;
            CalculateDateRanges();
        }
    }
}