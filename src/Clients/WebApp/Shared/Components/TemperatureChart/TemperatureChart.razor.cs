using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Axes;
using ChartJs.Blazor.Common.Enums;
using ChartJs.Blazor.Common.Time;
using ChartJs.Blazor.LineChart;
using ChartJs.Blazor.Util;
using DevExpress.Blazor.Internal;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Queries.SharedModels;
using SmartHome.Clients.WebApp.Shared.Components.DateRangeChart;

namespace SmartHome.Clients.WebApp.Shared.Components.TemperatureChart
{
    public class TemperatureChartComponent: BaseDateRangeChart<TemperatureVm, TemperatureAggregatesVm>
    {
        protected override Func<DateTime?, DateTime?, DateRangeGranulation?, Task<IEnumerable<IDataset>>>
            GetDataSetsConverter()
        {
           return
                async (fromDate, toDate, granulation) =>
                {
                    var data = await LoadData(fromDate, toDate, granulation);
                    var dataSets = new List<LineDataset<TimePoint>>
                    {
                        new()
                        {
                            Label = "Average temperature",
                            BackgroundColor = ColorUtil.FromDrawingColor(Color.Green),
                            BorderColor = ColorUtil.FromDrawingColor(Color.Green),
                            Fill = FillingMode.Disabled
                        },
                        new()
                        {
                            Label = "Maximum temperature",
                            BackgroundColor = ColorUtil.FromDrawingColor(Color.Red),
                            BorderColor = ColorUtil.FromDrawingColor(Color.Red),
                            Fill = FillingMode.Disabled
                        },
                        new()
                        {
                            Label = "Minimum temperature",
                            BackgroundColor = ColorUtil.FromDrawingColor(Color.Blue),
                            BorderColor = ColorUtil.FromDrawingColor(Color.Blue),
                            Fill = FillingMode.Disabled
                        }
                    };

                    foreach (var temperatureVm in data)
                    {
                        dataSets[0].Add(new TimePoint(temperatureVm.Timestamp, temperatureVm.Temperature));
                        dataSets[1].Add(new TimePoint(temperatureVm.Timestamp, temperatureVm.MaxTemperature));
                        dataSets[2].Add(new TimePoint(temperatureVm.Timestamp, temperatureVm.MinTemperature));
                    }

                    return dataSets;
                };
        }

        protected override Func<DateTime?, DateTime?, DateRangeGranulation?, Task<IEnumerable<ChartSummary>>>
            GetSummaryConverter()
        {
            return
                async (fromDate, toDate, granulation) =>
                {
                    var data = await LoadSummary(fromDate, toDate, granulation);
                    if (data == null)
                    {
                        return Enumerable.Empty<ChartSummary>();
                    }
                    return new List<ChartSummary>
                    {
                        new ChartSummary
                        {
                            Header = "Maximum temperature: ",
                            Value =
                                $"{data.MaxTemperature.ToString() ?? "-"} °C at {data.MaxTemperatureTimestamp?.ToLocalTime():dd.MM.yyyy HH:mm}"
                        },
                        new ChartSummary
                        {
                            Header = "Minimum temperature: ",
                            Value =
                                $"{data.MinTemperature.ToString() ?? "-"} °C at {data.MinTemperatureTimestamp?.ToLocalTime():dd.MM.yyyy HH:mm}"
                        }
                    };
                };
        }


        protected override ConfigBase GetConfig()
        {
            var defaultConfig = base.GetConfig() as LineConfig;
            defaultConfig!.Options.Scales.YAxes = new List<CartesianAxis>
            {
                new LinearCartesianAxis
                {
                    ScaleLabel = new ScaleLabel
                    {
                        LabelString = "Temperature [°C]",
                        Display = true
                    }
                }
            };
            return defaultConfig;
        }
    }
}