using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Events.Devices.Shared.DeviceCreated;
using SmartHome.Domain.Entities.Devices.Shared;

namespace SmartHome.Application.Events.Devices.Shared.DeviceCreated
{
    public class DeviceCreatedEventUpdateDbHandler : INotificationHandler<DeviceCreatedEvent>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DeviceCreatedEventUpdateDbHandler(IApplicationDbContext applicationDbContext,
            IDateTimeProvider dateTimeProvider)
        {
            _applicationDbContext = applicationDbContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public Task Handle(DeviceCreatedEvent @event, CancellationToken cancellationToken)
        {
            var device = _applicationDbContext.Device.FirstOrDefault(x => x.DeviceId == @event.Source);
            if (device == null)
            {
                _applicationDbContext.Device.Add(new Device
                {
                    DeviceId = @event.Source,
                    DeviceName = @event.Source,
                    IsOnline = true,
                    LastStatusUpdate = _dateTimeProvider.GetUtcNow()
                });

                return Task.CompletedTask;
            }

            device.IsDeleted = false;
            device.LastStatusUpdate = _dateTimeProvider.GetUtcNow();
            device.Online();

            return _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}