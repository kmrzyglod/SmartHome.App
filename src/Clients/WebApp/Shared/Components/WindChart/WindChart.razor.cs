using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Axes;
using ChartJs.Blazor.Common.Axes.Ticks;
using ChartJs.Blazor.Common.Enums;
using ChartJs.Blazor.Common.Handlers;
using ChartJs.Blazor.Common.Time;
using ChartJs.Blazor.Interop;
using ChartJs.Blazor.LineChart;
using ChartJs.Blazor.Util;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Queries.WeatherStation.GetWindParameters;
using SmartHome.Clients.WebApp.Shared.Components.DateRangeChart;
using SmartHome.Domain.Enums;

namespace SmartHome.Clients.WebApp.Shared.Components.WindChart
{
    public class WindChartComponent : BaseDateRangeChart<WindParametersVm>
    {
        private const string WIND_DIRECTION_Y_AXIS_ID = "WIND_DIRECTION_Y_AXIS_ID";

        protected override Func<DateTime?, DateTime?, DateRangeGranulation?, Task<IEnumerable<IDataset>>>
            GetDataSetsConverter()
        {
            Func<DateTime?, DateTime?, DateRangeGranulation?, Task<IEnumerable<IDataset>>> fnc =
                async (fromDate, toDate, granulation) =>
                {
                    var data = await LoadData(fromDate, toDate, granulation);
                    var lineDataSets = new List<LineDataset<TimePoint>>
                    {
                        new()
                        {
                            Label = "Average wind speed",
                            BackgroundColor = ColorUtil.FromDrawingColor(Color.Green),
                            BorderColor = ColorUtil.FromDrawingColor(Color.Green),
                            Fill = FillingMode.Disabled
                        },
                        new()
                        {
                            Label = "Maximum wind speed",
                            BackgroundColor = ColorUtil.FromDrawingColor(Color.Red),
                            BorderColor = ColorUtil.FromDrawingColor(Color.Red),
                            Fill = FillingMode.Disabled
                        },
                        new()
                        {
                            Label = "Minimum wind speed",
                            BackgroundColor = ColorUtil.FromDrawingColor(Color.Blue),
                            BorderColor = ColorUtil.FromDrawingColor(Color.Blue),
                            Fill = FillingMode.Disabled
                        },
                        new()
                        {
                            Label = "Wind direction",
                            BackgroundColor = ColorUtil.FromDrawingColor(Color.Orange),
                            BorderColor = ColorUtil.FromDrawingColor(Color.Orange),
                            Fill = FillingMode.Disabled,
                            ShowLine = false,
                            YAxisId = WIND_DIRECTION_Y_AXIS_ID,
                            PointRadius = 6
                        }
                    };

                    foreach (var windParametersVm in data)
                    {
                        lineDataSets[0]
                            .Add(new TimePoint(windParametersVm.Timestamp, windParametersVm.AverageWindSpeed));
                        lineDataSets[1].Add(new TimePoint(windParametersVm.Timestamp, windParametersVm.MaxWindSpeed));
                        lineDataSets[2].Add(new TimePoint(windParametersVm.Timestamp, windParametersVm.MinWindSpeed));
                        lineDataSets[3].Add(new TimePoint(windParametersVm.Timestamp,
                            (double) windParametersVm.MostFrequentWindDirection));
                    }

                    return lineDataSets;
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
                        LabelString = "Wind speed [m/s]",
                        Display = true
                    },
                    Position = Position.Left
                },
                new LinearCartesianAxis
                {
                    ScaleLabel = new ScaleLabel
                    {
                        LabelString = "Wind direction",
                        Display = true
                    },
                    ID = WIND_DIRECTION_Y_AXIS_ID,
                    Position = Position.Right,
                    Ticks = new LinearCartesianTicks
                    {
                        StepSize = 1.0,
                        Callback = new DelegateHandler<AxisTickCallback>((value, _, _) =>
                            ((WindDirection) value.ToObject<int>()).ToString())
                    }
                }
            };
            return defaultConfig;
        }
    }
}