using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Interfaces.DateTime;
using SmartHome.Application.Interfaces.DbContext;

namespace SmartHome.Application.Queries.Devices.Weather.GetInsolation
{
    public class GetInsolationQueryHandler : IRequestHandler<GetInsolationQuery, List<InsolationVm>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetInsolationQueryHandler(IApplicationDbContext applicationDbContext, IDateTimeProvider dateTimeProvider)
        {
            _applicationDbContext = applicationDbContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public Task<List<InsolationVm>> Handle(GetInsolationQuery request, CancellationToken cancellationToken)
        {
            var currentDate = _dateTimeProvider.GetUtcNow();
            request.WithDefaultValues(currentDate.AddDays(-2), currentDate);

            return _applicationDbContext
                .WeatherStationSun
                .Where(x => x.MeasurementStartTime >= request.From && x.MeasurementEndTime <= request.To)
                .Select(x => new InsolationVm
                {
                    Timestamp = x.MeasurementEndTime,
                    LightLevel = x.LightLevelInLux
                })
                .OrderBy(x => x.Timestamp)
                .ToListAsync(cancellationToken);
        }
    }
}