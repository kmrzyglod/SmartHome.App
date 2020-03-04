using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartHome.Application.Interfaces.DateTime;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Domain.Entities.Devices.Shared;

namespace SmartHome.Application.Events.Devices.Shared.Diagnostic
{
    public class DiagnosticEventUpdateDbHandler : INotificationHandler<DiagnosticEvent>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ILogger _logger;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DiagnosticEventUpdateDbHandler(IApplicationDbContext applicationDbContext, ILogger logger, IDateTimeProvider dateTimeProvider)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
        }

        public Task Handle(DiagnosticEvent @event, CancellationToken cancellationToken)
        {
            var device = _applicationDbContext.Device.FirstOrDefault(x => x.DeviceId == @event.Source);
            if (device == null)
            {
                _logger.LogWarning($"Device: {@event.Source} not found in db. Skipping status update for this device.");
                return Task.CompletedTask;
            }

            device.Online();
            device.LastStatusUpdate = _dateTimeProvider.GetUtcNow();

            _applicationDbContext.DeviceStatuses.Add(new DeviceStatus
            {
                Device = device,
                FreeHeapMemory = @event.FreeHeapMemory,
                GatewayIp = @event.GatewayIp,
                Ip = @event.Ip,
                Rssi = @event.Rssi,
                Ssid = @event.Ssid,
                Timestamp = _dateTimeProvider.GetUtcNow()
            });

            return _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}