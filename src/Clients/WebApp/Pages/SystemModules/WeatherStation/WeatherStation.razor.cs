using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SmartHome.Application.Shared.Commands.Devices.Shared.Ping;
using SmartHome.Application.Shared.Commands.Devices.Shared.SendDiagnosticData;
using SmartHome.Application.Shared.Events.Devices.Shared.Diagnostic;
using SmartHome.Application.Shared.Events.Devices.WeatherStation.Telemetry;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.Queries.General.GetDeviceStatus;
using SmartHome.Application.Shared.Queries.General.GetEvents;
using SmartHome.Clients.WebApp.Helpers;
using SmartHome.Clients.WebApp.Services.Devices;
using SmartHome.Clients.WebApp.Services.EventLog;
using SmartHome.Clients.WebApp.Services.Shared.CommandsExecutor;
using SmartHome.Clients.WebApp.Services.Shared.NotificationsHub;

namespace SmartHome.Clients.WebApp.Pages.SystemModules.WeatherStation
{
    public class WeatherStationView : ComponentBase, IDisposable
    {
        protected const string DEVICE_ID = "devices/esp32-weather-station";

        protected bool IsCurrentWeatherDataAvailable;

        [Inject]
        protected INotificationsHub NotificationsHub { get; set; } = null!;

        [Inject]
        protected IDevicesService DevicesService { get; set; } = null!;

        [Inject]
        protected IEventLogService EventLogService { get; set; } = null!;

        [Inject]
        protected IDateTimeProvider DateTimeProvider { get; set; } = null!;

        [Inject]
        protected ICommandsExecutor CommandsExecutor { get; set; } = null!;

        protected DeviceStatusVm DeviceDetails { get; set; } = new();
        protected WeatherTelemetryEvent WeatherData { get; set; } = new();

        private string NotificationHubSubscriptionId { get; } = Guid.NewGuid()
            .ToString();

        private async Task UpdateDeviceDetails()
        {
            DeviceDetails = await DevicesService.GetDeviceStatus(new GetDeviceStatusQuery
            {
                DeviceId = DEVICE_ID
            });
        }

        protected Task PingDevice(string deviceId)
        {
            return CommandsExecutor.ExecuteCommand(command => DevicesService.Ping(command), new PingCommand
            {
                TargetDeviceId = deviceId
            }, 15);
        }

        protected Task RefreshStatus(string deviceId)
        {
            return CommandsExecutor.ExecuteCommand(command => DevicesService.SendDiagnosticData(command), new SendDiagnosticDataCommand
            {
                TargetDeviceId = deviceId
            }, 30);
        }

        private void SubscribeToDiagnosticDataNotifications()
        {
            NotificationsHub.Subscribe<DiagnosticEvent>(NotificationHubSubscriptionId, evt =>
            {
                if (evt.Source != DEVICE_ID)
                {
                    return Task.CompletedTask;
                }

                DeviceDetails = new DeviceStatusVm
                {
                    DeviceName = DeviceDetails.DeviceName,
                    IsOnline = DeviceDetails.IsOnline,
                    Ssid = evt.Ssid,
                    Rssi = evt.Rssi,
                    Ip = evt.Ip,
                    GatewayIp = evt.GatewayIp,
                    FreeHeapMemory = evt.FreeHeapMemory,
                    LastStatusUpdate = DateTimeProvider.GetUtcNow()
                };
                
                StateHasChanged();
                
                return Task.CompletedTask;
            });
        }

        private void SubscribeToWeatherDataNotifications()
        {
            NotificationsHub.Subscribe<WeatherTelemetryEvent>(NotificationHubSubscriptionId, evt =>
            {
                if (evt.Source != DEVICE_ID)
                {
                    return Task.CompletedTask;
                }

                WeatherData = evt;
                StateHasChanged();
                return Task.CompletedTask;
            });
        }

        private async Task UpdateWeatherData()
        {
            var result = await EventLogService.GetEvents(new GetEventsQuery
            {
                EventName = nameof(WeatherTelemetryEvent),
                PageNumber = 1,
                PageSize = 1,
                From = DateTimeProvider.GetUtcNow()
                    .AddDays(-1),
                Source = DEVICE_ID
            });

            if (result.ResultTotalCount == 0)
            {
                IsCurrentWeatherDataAvailable = false;
                return;
            }

            WeatherData = JsonSerializerHelpers.DeserializeFromObject<WeatherTelemetryEvent>(result.Result.First()
                .EventData!);

            IsCurrentWeatherDataAvailable = true;
        }

        protected override async Task OnInitializedAsync()
        {
            SubscribeToDiagnosticDataNotifications();
            SubscribeToWeatherDataNotifications();
            await Task.WhenAll(UpdateDeviceDetails(), UpdateWeatherData());
            await base.OnInitializedAsync();
        }

        public void Dispose()
        {
            NotificationsHub.Unsubscribe(NotificationHubSubscriptionId);
        }
    }
}