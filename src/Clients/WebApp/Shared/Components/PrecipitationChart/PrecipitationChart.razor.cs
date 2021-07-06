using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Axes;
using ChartJs.Blazor.Common.Enums;
using ChartJs.Blazor.Common.Time;
using ChartJs.Blazor.Util;
using Microsoft.AspNetCore.Components;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Queries.WeatherStation.GetPrecipitation;
using SmartHome.Clients.WebApp.Shared.Components.DateRangeChart;

namespace SmartHome.Clients.WebApp.Shared.Components.PrecipitationChart
{
    public class PrecipitationChartComponent : BaseDateRangeChart<PrecipitationVm>
    {
        protected override Func<DateTime?, DateTime?, DateRangeGranulation?, Task<IEnumerable<IDataset>>> GetDataSetsConverter()
        {
            Func<DateTime?, DateTime?, DateRangeGranulation?, Task<IEnumerable<IDataset>>> fnc =
                async (DateTime? fromDate, DateTime? toDate, DateRangeGranulation? granulation) =>
                {
                    var data = await LoadData(fromDate, toDate, granulation);
                    var dataSets = new List<BarDataset<TimePoint>>
                    {
                        new BarDataset<TimePoint>
                        {
                            Label = "Precipitation",
                            BackgroundColor = ColorUtil.FromDrawingColor(Color.Blue),
                            BorderColor = ColorUtil.FromDrawingColor(Color.Blue),
                        }
                    };
                    
                    foreach (var precipitationVm in data)
                    {
                        dataSets[0].Add(new TimePoint(precipitationVm.Timestamp, precipitationVm.RainSum));
                    }
                    
                    return dataSets;
                };

            return fnc;

        }
        protected override ConfigBase GetConfig()
        {
            return new BarConfig
            {
                Options = new BarOptions
                {
                    Responsive = true,
                    Tooltips = new Tooltips
                    {
                        Mode = InteractionMode.Nearest,
                        Intersect = true
                    },
                    Hover = new Hover
                    {
                        Mode = InteractionMode.Nearest,
                        Intersect = true
                    },
                    Scales = new BarScales
                    {
                        XAxes = new List<CartesianAxis>
                        {
                            new TimeAxis
                            {
                                ScaleLabel = new ScaleLabel
                                {
                                    LabelString = "Date",
                                    Display = true,
                                },
                                Time = new TimeOptions
                                {
                                    TooltipFormat = "YYYY-MM-DD HH:mm",
                                    DisplayFormats = new Dictionary<TimeMeasurement, string>
                                    {
                                        {TimeMeasurement.Minute, "HH:mm"},
                                        {TimeMeasurement.Hour, "MM-DD HH:mm"}, 
                                        {TimeMeasurement.Day, "YYYY-MM-DD"},
                                        {TimeMeasurement.Month, "YYYY-MM"},
                                        {TimeMeasurement.Year, "YYYY"},
                                    },
                                    Unit = TimeMeasurement.Hour
                                },
                            }
                        },
                        YAxes = new List<CartesianAxis>
                        {
                            new LinearCartesianAxis
                            {
                                ScaleLabel = new ScaleLabel
                                {
                                    LabelString = "Precipitation [mm]",
                                    Display = true
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}