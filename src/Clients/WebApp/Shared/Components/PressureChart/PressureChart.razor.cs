using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Axes;
using ChartJs.Blazor.Common.Enums;
using ChartJs.Blazor.Common.Time;
using ChartJs.Blazor.LineChart;
using ChartJs.Blazor.Util;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Queries.WeatherStation.GetPressure;
using SmartHome.Clients.WebApp.Shared.Components.DateRangeChart;

namespace SmartHome.Clients.WebApp.Shared.Components.PressureChart
{
    public class PressureChartComponent : BaseDateRangeChart<PressureVm>
    {
        protected override Func<DateTime?, DateTime?, DateRangeGranulation?, Task<IEnumerable<IDataset>>>
            GetDataSetsConverter()
        {
            Func<DateTime?, DateTime?, DateRangeGranulation?, Task<IEnumerable<IDataset>>> fnc =
                async (fromDate, toDate, granulation) =>
                {
                    var data = await LoadData(fromDate, toDate, granulation);
                    var dataSets = new List<LineDataset<TimePoint>>
                    {
                        new()
                        {
                            Label = "Average pressure",
                            BackgroundColor = ColorUtil.FromDrawingColor(Color.Green),
                            BorderColor = ColorUtil.FromDrawingColor(Color.Green),
                            Fill = FillingMode.Disabled
                        },
                        new()
                        {
                            Label = "Maximum pressure",
                            BackgroundColor = ColorUtil.FromDrawingColor(Color.Red),
                            BorderColor = ColorUtil.FromDrawingColor(Color.Red),
                            Fill = FillingMode.Disabled
                        },
                        new()
                        {
                            Label = "Minimum pressure",
                            BackgroundColor = ColorUtil.FromDrawingColor(Color.Blue),
                            BorderColor = ColorUtil.FromDrawingColor(Color.Blue),
                            Fill = FillingMode.Disabled
                        }
                    };

                    foreach (var pressureVm in data)
                    {
                        dataSets[0].Add(new TimePoint(pressureVm.Timestamp, pressureVm.Pressure));
                        dataSets[1].Add(new TimePoint(pressureVm.Timestamp, pressureVm.MaxPressure));
                        dataSets[2].Add(new TimePoint(pressureVm.Timestamp, pressureVm.MinPressure));
                    }

                    return dataSets;
                };

            return fnc;
        }

        protected override  ConfigBase GetConfig()
        {
            var defaultConfig = base.GetConfig() as LineConfig;
            defaultConfig!.Options.Scales.YAxes = new List<CartesianAxis>
            {
                new LinearCartesianAxis
                {
                    ScaleLabel = new ScaleLabel
                    {
                        LabelString = "Pressure [hPa]",
                        Display = true
                    }
                }
            };
            return defaultConfig;
        }
    }
}