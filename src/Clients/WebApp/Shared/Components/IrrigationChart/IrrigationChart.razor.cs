using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Axes;
using ChartJs.Blazor.Common.Enums;
using ChartJs.Blazor.Common.Time;
using ChartJs.Blazor.LineChart;
using ChartJs.Blazor.Util;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetIrrigationData;
using SmartHome.Clients.WebApp.Shared.Components.DateRangeChart;

namespace SmartHome.Clients.WebApp.Shared.Components.IrrigationChart
{
    public class IrrigationChartComponent : BaseDateRangeChart<IrrigationDataVm>
    {
        private const string TOTAL_WATER_VOLUME_Y_AXIS_ID = "TOTAL_WATER_VOLUME_Y_AXIS_ID";

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
                            Label = "Average water flow",
                            BackgroundColor = ColorUtil.FromDrawingColor(Color.Green),
                            BorderColor = ColorUtil.FromDrawingColor(Color.Green),
                            Fill = FillingMode.Disabled
                        },
                        new()
                        {
                            Label = "Maximum water flow",
                            BackgroundColor = ColorUtil.FromDrawingColor(Color.Red),
                            BorderColor = ColorUtil.FromDrawingColor(Color.Red),
                            Fill = FillingMode.Disabled
                        },
                        new()
                        {
                            Label = "Minimum water flow",
                            BackgroundColor = ColorUtil.FromDrawingColor(Color.Blue),
                            BorderColor = ColorUtil.FromDrawingColor(Color.Blue),
                            Fill = FillingMode.Disabled
                        }
                    };

                    var barDataSet = new BarDataset<TimePoint>
                    {
                        Label = "Total water volume",
                        BackgroundColor = ColorUtil.FromDrawingColor(Color.Orange),
                        BorderColor = ColorUtil.FromDrawingColor(Color.Orange),
                        YAxisId = TOTAL_WATER_VOLUME_Y_AXIS_ID
                    };

                    foreach (var irrigationDataVm in data)
                    {
                        lineDataSets[0]
                            .Add(new TimePoint(irrigationDataVm.Timestamp, irrigationDataVm.AverageWaterFlow));
                        lineDataSets[1].Add(new TimePoint(irrigationDataVm.Timestamp, irrigationDataVm.MaxWaterFlow));
                        lineDataSets[2].Add(new TimePoint(irrigationDataVm.Timestamp, irrigationDataVm.MinWaterFlow));
                        barDataSet.Add(new TimePoint(irrigationDataVm.Timestamp,
                            irrigationDataVm.TotalWaterVolume));
                    }

                    return lineDataSets.Select(x => x as IDataset).Append(barDataSet);
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
                        LabelString = "Water flow [l/min]",
                        Display = true
                    },
                    Position = Position.Left
                },
                new LinearCartesianAxis
                {
                    ScaleLabel = new ScaleLabel
                    {
                        LabelString = "Total water volume [l]",
                        Display = true
                    },
                    ID = TOTAL_WATER_VOLUME_Y_AXIS_ID,
                    Position = Position.Right
                }
            };
            return defaultConfig;
        }
    }
}