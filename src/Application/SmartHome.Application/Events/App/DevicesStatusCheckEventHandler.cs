using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Events.App;
using SmartHome.Application.Shared.Interfaces.DateTime;

namespace SmartHome.Application.Events.App
{
    public class DevicesStatusCheckEventHandler : INotificationHandler<DevicesStatusCheckEvent>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IApplicationDbContext _dbContext;

        public DevicesStatusCheckEventHandler(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task Handle(DevicesStatusCheckEvent notification, CancellationToken cancellationToken)
        {
            var devices = await _dbContext.Device.ToListAsync(cancellationToken);
            foreach (var device in devices.Where(device => (_dateTimeProvider.GetUtcNow() - device.LastStatusUpdate).Minutes > 65))
            {
                device.Offline();
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}