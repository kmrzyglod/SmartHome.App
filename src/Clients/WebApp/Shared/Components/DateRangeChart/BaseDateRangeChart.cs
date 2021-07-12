﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Axes;
using ChartJs.Blazor.Common.Enums;
using ChartJs.Blazor.Common.Time;
using ChartJs.Blazor.LineChart;
using Microsoft.AspNetCore.Components;
using SmartHome.Application.Shared.Enums;
using SmartHome.Clients.WebApp.Services.Shared.NotificationsHub;

namespace SmartHome.Clients.WebApp.Shared.Components.DateRangeChart
{
    public abstract class BaseDateRangeChart<TDataModel> : ComponentBase, IDisposable
    {
        [Parameter]
        public Func<DateTime?, DateTime?, DateRangeGranulation?, Task<IEnumerable<TDataModel>>> LoadData { get; set; } =
            null!;

        [Parameter] public bool LoadDataAfterInitialization { get; set; }
        [Parameter] public string NotificationHubEventName { get; set; } = null!;

        protected DateRangeChartComponent Chart { get; set; } = null!;

        [Inject] protected INotificationsHub NotificationsHub { get; set; } = null!;
        private string _notificationHubSubscriptionId { get; } = Guid.NewGuid().ToString();

        public virtual void Dispose()
        {
            NotificationsHub.Unsubscribe(_notificationHubSubscriptionId);
        }


        public Task UpdateData()
        {
            return Chart.UpdateData();
        }

        protected abstract Func<DateTime?, DateTime?, DateRangeGranulation?, Task<IEnumerable<IDataset>>>
            GetDataSetsConverter();

        protected virtual ConfigBase GetConfig()
        {
            return new LineConfig
            {
                Options = new LineOptions
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
                    Scales = new Scales
                    {
                        XAxes = new List<CartesianAxis>
                        {
                            new TimeAxis
                            {
                                ScaleLabel = new ScaleLabel
                                {
                                    LabelString = "Date",
                                    Display = true
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
                                        {TimeMeasurement.Year, "YYYY"}
                                    },
                                    Unit = TimeMeasurement.Hour
                                }
                            }
                        }
                    }
                }
            };
        }

        protected override void OnInitialized()
        {
            if (string.IsNullOrEmpty(NotificationHubEventName))
            {
                return;
            }
            NotificationsHub.Subscribe(NotificationHubEventName, _notificationHubSubscriptionId, async arg =>
            {
                if (!Chart.AutoUpdateCheckBox)
                {
                    return;
                }

                await UpdateData();
            });
        }
    }
}