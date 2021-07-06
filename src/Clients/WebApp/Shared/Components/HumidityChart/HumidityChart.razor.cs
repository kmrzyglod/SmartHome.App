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
using Microsoft.AspNetCore.Components;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Queries.SharedModels;
using SmartHome.Clients.WebApp.Shared.Components.DateRangeChart;

namespace SmartHome.Clients.WebApp.Shared.Components.HumidityChart
{
    public class HumidityChartComponent : BaseDateRangeChart<HumidityVm>
    {
        protected override  Func<DateTime?, DateTime?, DateRangeGranulation?, Task<IEnumerable<IDataset>>> GetDataSetsConverter()
        {
            Func<DateTime?, DateTime?, DateRangeGranulation?, Task<IEnumerable<IDataset>>> fnc =
                async (DateTime? fromDate, DateTime? toDate, DateRangeGranulation? granulation) =>
                {
                    var data = await LoadData(fromDate, toDate, granulation);
                    var dataSets = new List<LineDataset<TimePoint>>
                    {
                        new LineDataset<TimePoint>
                        {
                            Label = "Average Humidity",
                            BackgroundColor = ColorUtil.FromDrawingColor(Color.Green),
                            BorderColor = ColorUtil.FromDrawingColor(Color.Green),
                            Fill = FillingMode.Disabled
                        },
                        new LineDataset<TimePoint>
                        {
                            Label = "Maximum humidity",
                            BackgroundColor = ColorUtil.FromDrawingColor(Color.Red),
                            BorderColor = ColorUtil.FromDrawingColor(Color.Red),
                            Fill = FillingMode.Disabled
                        },
                        new LineDataset<TimePoint>
                        {
                            Label = "Minimum humidity",
                            BackgroundColor = ColorUtil.FromDrawingColor(Color.Blue),
                            BorderColor = ColorUtil.FromDrawingColor(Color.Blue),
                            Fill = FillingMode.Disabled,
                        }
                    };
                    
                    foreach (var humidityVm in data)
                    {
                        dataSets[0].Add(new TimePoint(humidityVm.Timestamp, humidityVm.Humidity));
                        dataSets[1].Add(new TimePoint(humidityVm.Timestamp, humidityVm.MaxHumidity));
                        dataSets[2].Add(new TimePoint(humidityVm.Timestamp, humidityVm.MinHumidity));
                    }
                    
                    return dataSets;
                };

            return fnc;

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
                        LabelString = "Humidity [%]",
                        Display = true
                    }
                }
            };
            return defaultConfig;
        }
    }
}