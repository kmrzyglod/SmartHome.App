using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartHome.Application.Interfaces.DateTime;
using SmartHome.Application.Interfaces.DbContext;

namespace SmartHome.Application.Events.Devices.Shared.DeviceDisconnected
{
    public class DeviceDisconnectedEventUpdateDbHandler : INotificationHandler<DeviceDisconnectedEvent>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ILogger _logger;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DeviceDisconnectedEventUpdateDbHandler(IApplicationDbContext applicationDbContext, ILogger logger, IDateTimeProvider dateTimeProvider)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
        }

        public Task Handle(DeviceDisconnectedEvent @event, CancellationToken cancellationToken)
        {
            var device = _applicationDbContext.Device.FirstOrDefault(x => x.DeviceId == @event.Source);
            if (device == null)
            {
                _logger.LogWarning($"Device: {@event.Source} not found in db. Skipping status update for this device.");
                return Task.CompletedTask;
            }

            device.LastStatusUpdate = _dateTimeProvider.GetUtcNow();
            device.Offline();

            return _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}