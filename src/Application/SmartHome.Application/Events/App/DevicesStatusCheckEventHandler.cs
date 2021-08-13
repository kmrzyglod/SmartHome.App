using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Interfaces.EventStore;
using SmartHome.Application.Shared.Events.App;
using SmartHome.Application.Shared.Events.Devices.Shared.DeviceDisconnected;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.Queries.General.GetEvents;

namespace SmartHome.Application.Events.App
{
    public class DevicesStatusCheckEventHandler : INotificationHandler<DevicesStatusCheckEvent>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IApplicationDbContext _dbContext;
        private readonly IEventStoreClient _eventStore;

        public DevicesStatusCheckEventHandler(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider,
            IEventStoreClient eventStore)
        {
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;
            _eventStore = eventStore;
        }

        public async Task Handle(DevicesStatusCheckEvent notification, CancellationToken cancellationToken)
        {
            var currentTimestamp = _dateTimeProvider.GetUtcNow();
            var devices = await _dbContext.Device.ToListAsync(cancellationToken);
            var deviceLastEvents = (await Task.WhenAll(devices.Select(x => _eventStore.FindEventsByCriteriaAsync(
                    new GetEventsQuery
                    {
                        Source = x.DeviceId,
                        From = currentTimestamp.AddHours(-1)
                    }, cancellationToken))))
                .SelectMany(x => x.Result)
                .ToList();

            foreach (var device in devices)
            {
                var deviceLastEvt = deviceLastEvents.FirstOrDefault(x => x.Source == device.DeviceId);

                if (deviceLastEvt == null || deviceLastEvt.EventName == nameof(DeviceDisconnectedEvent))
                {
                    device.Offline();
                    continue;
                }

                device.LastStatusUpdate = deviceLastEvt.Timestamp;
                device.Online();
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}