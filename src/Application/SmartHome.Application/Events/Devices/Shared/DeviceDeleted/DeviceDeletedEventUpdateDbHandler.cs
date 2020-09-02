using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Events.Devices.Shared.DeviceDeleted;

namespace SmartHome.Application.Events.Devices.Shared.DeviceDeleted
{
    public class DeviceDeletedEventUpdateDbHandler : INotificationHandler<DeviceDeletedEvent>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ILogger _logger;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DeviceDeletedEventUpdateDbHandler(IApplicationDbContext applicationDbContext, ILogger logger, IDateTimeProvider dateTimeProvider)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
        }

        public Task Handle(DeviceDeletedEvent @event, CancellationToken cancellationToken)
        {
            var device = _applicationDbContext.Device.FirstOrDefault(x => x.DeviceId == @event.Source);
            if (device == null)
            {
                _logger.LogWarning($"Device: {@event.Source} not found in db. Skipping status update for this device.");
                return Task.CompletedTask;
            }

            device.IsDeleted = true;
            device.LastStatusUpdate = _dateTimeProvider.GetUtcNow();

            return _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}