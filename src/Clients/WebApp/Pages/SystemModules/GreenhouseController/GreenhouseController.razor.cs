using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Radzen;
using SmartHome.Application.Shared.Commands.Devices.Shared.Ping;
using SmartHome.Application.Shared.Commands.Devices.Shared.SendDiagnosticData;
using SmartHome.Application.Shared.Events.Devices.GreenhouseController.Door;
using SmartHome.Application.Shared.Events.Devices.GreenhouseController.Telemetry;
using SmartHome.Application.Shared.Events.Devices.Shared.Diagnostic;
using SmartHome.Application.Shared.Events.Devices.WeatherStation.Telemetry;
using SmartHome.Application.Shared.Events.Devices.WindowsController.Telemetry;
using SmartHome.Application.Shared.Events.Devices.WindowsController.WindowClosed;
using SmartHome.Application.Shared.Events.Devices.WindowsController.WindowOpened;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.Queries.General.GetDeviceStatus;
using SmartHome.Application.Shared.Queries.General.GetEvents;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetWindowsStatus;
using SmartHome.Clients.WebApp.Helpers;
using SmartHome.Clients.WebApp.Services.Analytics;
using SmartHome.Clients.WebApp.Services.Devices;
using SmartHome.Clients.WebApp.Services.EventLog;
using SmartHome.Clients.WebApp.Services.Shared.CommandsExecutor;
using SmartHome.Clients.WebApp.Shared.Components;

namespace SmartHome.Clients.WebApp.Pages.SystemModules.GreenhouseController
{
    public class GreenhouseControllerComponent : ComponentWithNotificationHub
    {
        protected const string GREENHOUSE_CONTROLLER_DEVICE_ID = "devices/esp32-greenhouse";
        protected const string WINDOWS_CONTROLLER_DEVICE_ID = "devices/esp32-windows-controller";

        protected bool IsCurrentGreenhouseDataAvailable;


        public GreenhouseControllerComponent()
        {
            _buttonsState.TryAdd($"{GREENHOUSE_CONTROLLER_DEVICE_ID}_ping", false);
            _buttonsState.TryAdd($"{GREENHOUSE_CONTROLLER_DEVICE_ID}_refresh", false);
            _buttonsState.TryAdd($"{WINDOWS_CONTROLLER_DEVICE_ID}_ping", false);
            _buttonsState.TryAdd($"{WINDOWS_CONTROLLER_DEVICE_ID}_refresh", false);
        }

        [Inject] protected IDevicesService DevicesService { get; set; } = null!;
       
        [Inject] protected IGreenhouseService GreenhouseService { get; set; } = null!;

        [Inject] protected IEventLogService EventLogService { get; set; } = null!;

        [Inject] protected IDateTimeProvider DateTimeProvider { get; set; } = null!;

        [Inject] protected ICommandsExecutor CommandsExecutor { get; set; } = null!;

        [Inject] protected NotificationService ToastrNotificationService { get; set; } = null!;

        protected DeviceStatusVm GreenhouseControllerDeviceDetails { get; set; } = new();
        protected DeviceStatusVm WindowsControllerDeviceDetails { get; set; } = new();
        protected GreenhouseControllerTelemetryEvent GreenhouseControllerData { get; set; } = new();
        protected WindowsStatusVm WindowsStatus { get; set; } = new();

        private readonly ConcurrentDictionary<string, bool> _buttonsState = new();

        protected bool IsPingButtonDisabled(string deviceId)
        {
            return _buttonsState.TryGetValue($"{deviceId}_ping", out var state) && state;
        }

        protected bool IsRefreshButtonDisabled(string deviceId)
        {
            return _buttonsState.TryGetValue($"{deviceId}_refresh", out var state) && state;
        }

        private void SetButtonState(string buttonKey, bool state)
        {
            _buttonsState.TryUpdate(buttonKey, state, !state);
        }

        private async Task UpdateDeviceDetails()
        {
            var results = await Task.WhenAll(DevicesService.GetDeviceStatus(new GetDeviceStatusQuery
            {
                DeviceId = GREENHOUSE_CONTROLLER_DEVICE_ID
            }), DevicesService.GetDeviceStatus(new GetDeviceStatusQuery
            {
                DeviceId = WINDOWS_CONTROLLER_DEVICE_ID
            }));

            GreenhouseControllerDeviceDetails = results[0];
            WindowsControllerDeviceDetails = results[1];

            if (!GreenhouseControllerDeviceDetails.IsOnline)
            {
                SetButtonState($"{GREENHOUSE_CONTROLLER_DEVICE_ID}_ping", true);
                SetButtonState($"{GREENHOUSE_CONTROLLER_DEVICE_ID}_refresh", true);
            }
            else
            {
                SetButtonState($"{GREENHOUSE_CONTROLLER_DEVICE_ID}_ping", false);
                SetButtonState($"{GREENHOUSE_CONTROLLER_DEVICE_ID}_refresh", false);
            }
        }

        protected Task PingDevice(string deviceId)
        {
            SetButtonState($"{deviceId}_ping", true);
            return CommandsExecutor.ExecuteCommand(command => DevicesService.Ping(command), new PingCommand
            {
                TargetDeviceId = deviceId
            }, 15, _ =>
            {
                SetButtonState($"{deviceId}_ping", false);
                StateHasChanged();
            });
        }

        protected Task RefreshStatus(string deviceId)
        {
            SetButtonState($"{deviceId}_refresh", true);
            return CommandsExecutor.ExecuteCommand(command => DevicesService.SendDiagnosticData(command),
                new SendDiagnosticDataCommand
                {
                    TargetDeviceId = deviceId
                }, 30, _ =>
                {
                    SetButtonState($"{deviceId}_refresh", false);
                    StateHasChanged();
                });
        }

        private void SubscribeToDiagnosticDataNotifications()
        {
            NotificationsHub.Subscribe<DiagnosticEvent>(NotificationHubSubscriptionId, evt =>
            {
                if (evt.Source != GREENHOUSE_CONTROLLER_DEVICE_ID && evt.Source != WINDOWS_CONTROLLER_DEVICE_ID)
                {
                    return Task.CompletedTask;
                }

                switch (evt.Source)
                {
                    case GREENHOUSE_CONTROLLER_DEVICE_ID:
                        GreenhouseControllerDeviceDetails = new DeviceStatusVm
                        {
                            DeviceName = GreenhouseControllerDeviceDetails.DeviceName,
                            IsOnline = true,
                            Ssid = evt.Ssid,
                            Rssi = evt.Rssi,
                            Ip = evt.Ip,
                            GatewayIp = evt.GatewayIp,
                            FreeHeapMemory = evt.FreeHeapMemory,
                            LastStatusUpdate = DateTimeProvider.GetUtcNow()
                        };
                        break;
                    case WINDOWS_CONTROLLER_DEVICE_ID:
                        WindowsControllerDeviceDetails = new DeviceStatusVm
                        {
                            DeviceName = WindowsControllerDeviceDetails.DeviceName,
                            IsOnline = true,
                            Ssid = evt.Ssid,
                            Rssi = evt.Rssi,
                            Ip = evt.Ip,
                            GatewayIp = evt.GatewayIp,
                            FreeHeapMemory = evt.FreeHeapMemory,
                            LastStatusUpdate = DateTimeProvider.GetUtcNow()
                        };
                        break;
                }

                StateHasChanged();

                return Task.CompletedTask;
            });
        }

        private void SubscribeToGreenhouseDataNotifications()
        {
            NotificationsHub.Subscribe<GreenhouseControllerTelemetryEvent>(NotificationHubSubscriptionId, evt =>
            {
                if (evt.Source != GREENHOUSE_CONTROLLER_DEVICE_ID)
                {
                    return Task.CompletedTask;
                }

                GreenhouseControllerData = evt;
                WindowsStatus.Door = evt.IsDoorOpen;
                StateHasChanged();
                ToastrNotificationService.Notify(NotificationSeverity.Info, "", "Greenhouse data refreshed");
                return Task.CompletedTask;
            });
        }

        private void SubscribeToWindowsControllerNotifications()
        {
            NotificationsHub.Subscribe<WindowsControllerTelemetryEvent>(NotificationHubSubscriptionId, evt =>
            {
                if (evt.Source != WINDOWS_CONTROLLER_DEVICE_ID)
                {
                    return Task.CompletedTask;
                }

                WindowsStatus.Window1 = evt.WindowsStatus[0];
                WindowsStatus.Window2 = evt.WindowsStatus[1];
                StateHasChanged();
                ToastrNotificationService.Notify(NotificationSeverity.Info, "", "Door & windows states refreshed");
                return Task.CompletedTask;
            });

            NotificationsHub.Subscribe<DoorOpenedEvent>(NotificationHubSubscriptionId, evt =>
            {
                if (evt.Source != WINDOWS_CONTROLLER_DEVICE_ID)
                {
                    return Task.CompletedTask;
                }

                WindowsStatus.Door = true;
                StateHasChanged();
                ToastrNotificationService.Notify(NotificationSeverity.Info, "", "Door & windows states refreshed");
                return Task.CompletedTask;
            });

            NotificationsHub.Subscribe<DoorClosedEvent>(NotificationHubSubscriptionId, evt =>
            {
                if (evt.Source != WINDOWS_CONTROLLER_DEVICE_ID)
                {
                    return Task.CompletedTask;
                }

                WindowsStatus.Door = false;
                StateHasChanged();
                ToastrNotificationService.Notify(NotificationSeverity.Info, "", "Door & windows states refreshed");
                return Task.CompletedTask;
            });

            NotificationsHub.Subscribe<WindowOpenedEvent>(NotificationHubSubscriptionId, evt =>
            {
                if (evt.Source != WINDOWS_CONTROLLER_DEVICE_ID)
                {
                    return Task.CompletedTask;
                }

                switch (evt.WindowId)
                {
                    case 1:
                        WindowsStatus.Window1 = true;
                        break;
                    case 2:
                        WindowsStatus.Window2 = true;
                        break;
                }

                StateHasChanged();
                ToastrNotificationService.Notify(NotificationSeverity.Info, "", "Door & windows states refreshed");
                return Task.CompletedTask;
            });

            NotificationsHub.Subscribe<WindowClosedEvent>(NotificationHubSubscriptionId, evt =>
            {
                if (evt.Source != WINDOWS_CONTROLLER_DEVICE_ID)
                {
                    return Task.CompletedTask;
                }

                switch (evt.WindowId)
                {
                    case 1:
                        WindowsStatus.Window1 = false;
                        break;
                    case 2:
                        WindowsStatus.Window2 = false;
                        break;
                }

                StateHasChanged();
                ToastrNotificationService.Notify(NotificationSeverity.Info, "", "Door & windows states refreshed");
                return Task.CompletedTask;
            });
        }

        private async Task UpdateGreenhouseData()
        {
            var result = await EventLogService.GetEvents(new GetEventsQuery
            {
                EventName = nameof(GreenhouseControllerTelemetryEvent),
                PageNumber = 1,
                PageSize = 1,
                From = DateTimeProvider.GetUtcNow()
                    .AddDays(-1),
                Source = GREENHOUSE_CONTROLLER_DEVICE_ID
            });

            if (result.ResultTotalCount == 0)
            {
                IsCurrentGreenhouseDataAvailable = false;
                return;
            }

            GreenhouseControllerData = JsonSerializerHelpers.DeserializeFromObject<GreenhouseControllerTelemetryEvent>(result.Result.First()
                .EventData!);

            IsCurrentGreenhouseDataAvailable = true;
        }

        private async Task UpdateWindowsStatus()
        {
            WindowsStatus = await GreenhouseService.GetWindowsStatus(new GetWindowsStatusQuery { });
        }

        protected override async Task OnInitializedAsync()
        {
            SubscribeToDiagnosticDataNotifications();
            SubscribeToGreenhouseDataNotifications();
            SubscribeToWindowsControllerNotifications();
            await Task.WhenAll(UpdateDeviceDetails(), UpdateGreenhouseData(), UpdateWindowsStatus());
            await base.OnInitializedAsync();
        }
    }
}
