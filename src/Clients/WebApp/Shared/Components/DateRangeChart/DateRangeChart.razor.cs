using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChartJs.Blazor;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Enums;
using Microsoft.AspNetCore.Components;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Clients.WebApp.Shared.Components.DateRangePicker;

namespace SmartHome.Clients.WebApp.Shared.Components.DateRangeChart
{
    public class DateRangeChartComponent : ComponentBase
    {
        public bool AutoUpdateCheckBox;
        protected Chart Chart = null!;
        protected DateRangePickerBase DateRangePicker = null!;
        protected DateTime DefaultFromDateTime;
        protected DateRangeGranulation DefaultGranulation = DateRangeGranulation.Hour;
        protected DateTime DefaultToDateTime;
        [Inject] protected IDateTimeProvider DateTimeProvider { get; set; } = null!;

        [Parameter] public ConfigBase ChartConfig { get; set; } = null!;
        protected ConfigBase InternalChartConfig { get; set; } = null!;
        protected IEnumerable<ChartSummary> Summary { get; set; } = Enumerable.Empty<ChartSummary>();

        [Parameter]
        public Func<DateTime?, DateTime?, DateRangeGranulation?, Task<IEnumerable<IDataset>>> LoadData { get; set; }

        [Parameter]
        public Func<DateTime?, DateTime?, DateRangeGranulation?, Task<IEnumerable<ChartSummary>>> LoadSummary
        {
            get;
            set;
        }

        [Parameter] public bool LoadDataAfterInitialization { get; set; }

        protected async Task OnDatesRangeChanged(DateChangedEventArgs eventArgs)
        {
            await UpdateData();
        }

        public async Task UpdateData()
        {
            //Ugly but in .Net core 5.0 Blazor it's not possible to create generic component with constraint on type so 
            //this workaround is needed for accessing Data property which is not defined in ConfigBase bas defined in child classes 
            var granulation = DateRangePicker?.SelectedGranulationType.Type ?? DefaultGranulation;
            dynamic chartConfig = InternalChartConfig;
            chartConfig.Options.Scales.XAxes[0].Time.Unit = GetChartTimeMeasurement(granulation);
            chartConfig.Data.Datasets.Clear();
            foreach (var dataset in await LoadData(DateRangePicker?.FromDate ?? DefaultFromDateTime,
                DateRangePicker?.ToDate ?? DefaultToDateTime,
                granulation))
            {
                chartConfig.Data.Datasets.Add(dataset);
            }

            if (LoadSummary == null)
            {
                return;
            }

            Summary = await LoadSummary(DateRangePicker?.FromDate ?? DefaultFromDateTime,
                DateRangePicker?.ToDate ?? DefaultToDateTime,
                granulation);
        }

        protected override void OnInitialized()
        {
            DefaultFromDateTime = DateTimeProvider.GetUtcNow().Date.AddDays(-2);
            DefaultToDateTime = DateTimeProvider.GetUtcNow().Date.AddDays(1);
            InternalChartConfig = ChartConfig;
        }

        protected override async Task OnInitializedAsync()
        {
            if (LoadDataAfterInitialization)
            {
                await UpdateData();
            }
        }

        protected TimeMeasurement GetChartTimeMeasurement(DateRangeGranulation granulation)
        {
            return granulation switch
            {
                DateRangeGranulation.Raw => TimeMeasurement.Minute,
                DateRangeGranulation.FifteenMinutes => TimeMeasurement.Hour,
                DateRangeGranulation.HalfHour => TimeMeasurement.Hour,
                DateRangeGranulation.Hour => TimeMeasurement.Hour,
                DateRangeGranulation.ThreeHours => TimeMeasurement.Hour,
                DateRangeGranulation.SixHours => TimeMeasurement.Hour,
                DateRangeGranulation.Day => TimeMeasurement.Day,
                DateRangeGranulation.Month => TimeMeasurement.Month,
                DateRangeGranulation.Year => TimeMeasurement.Year,
                _ => TimeMeasurement.Hour
            };
        }
    }
}