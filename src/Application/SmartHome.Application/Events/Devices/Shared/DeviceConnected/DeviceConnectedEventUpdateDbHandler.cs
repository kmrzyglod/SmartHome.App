using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Logging;
using SmartHome.Application.Shared.Events.Devices.Shared.DeviceConnected;
using SmartHome.Application.Shared.Interfaces.DateTime;

namespace SmartHome.Application.Events.Devices.Shared.DeviceConnected
{
    public class DeviceConnectedEventUpdateDbHandler : INotificationHandler<DeviceConnectedEvent>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ILogger<LoggingContext> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DeviceConnectedEventUpdateDbHandler(IApplicationDbContext applicationDbContext, ILogger<LoggingContext> logger, IDateTimeProvider dateTimeProvider)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
        }

        public Task Handle(DeviceConnectedEvent @event, CancellationToken cancellationToken)
        {
            var device = _applicationDbContext.Device.FirstOrDefault(x => x.DeviceId == @event.Source);
            if (device == null)
            {
                _logger.LogWarning($"Device: {@event.Source} not found in db. Skipping status update for this device.");
                return Task.CompletedTask;
            }

            device.Online();
            device.LastStatusUpdate = _dateTimeProvider.GetUtcNow();
            return _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}