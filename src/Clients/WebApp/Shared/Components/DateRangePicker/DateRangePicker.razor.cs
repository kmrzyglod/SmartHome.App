using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Interfaces.DateTime;

namespace SmartHome.Clients.WebApp.Shared.Components.DateRangePicker
{
    public class DateRangePickerBase : ComponentBase
    {
        protected IEnumerable<GranulationType> GranulationTypes = GranulationType.Types;

        private GranulationType _selectedGranulationType =
            GranulationType.Types.First(x => x.Type == DateRangeGranulation.Hour);

        protected GranulationType SelectedGranulationType
        {
            get => _selectedGranulationType;
            set { _selectedGranulationType = value;
                DateChanged.InvokeAsync(new DateChangedEventArgs(FromDate, ToDate, value.Type));
            }
        }

        [Parameter] public EventCallback<DateChangedEventArgs> DateChanged { get; set; }

        [Parameter] public DateTime? DefaultFromDate { get; set; }

        [Parameter] public DateTime? DefaultToDate { get; set; }
        
        protected DateTime? FromDate { get; set; }

        protected DateTime? ToDate { get; set; }

        protected void OnFromDateChanged(DateTime? fromDate)
        {
            if (fromDate > ToDate)
            {
                return;
            }

            FromDate = fromDate;
            DateChanged.InvokeAsync(new DateChangedEventArgs(fromDate, ToDate, SelectedGranulationType.Type));
        }

        protected void OnToDateChanged(DateTime? toDate)
        {
            if (toDate < FromDate)
            {
                return;
            }

            ToDate = toDate;
            DateChanged.InvokeAsync(new DateChangedEventArgs(FromDate?.AddDays(1), toDate, SelectedGranulationType.Type));
        }

        protected override void OnInitialized()
        {
            FromDate = DefaultFromDate;
            ToDate = DefaultToDate;
        }
    }
}