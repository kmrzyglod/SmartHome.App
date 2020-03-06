using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Interfaces.DateTime;
using SmartHome.Application.Interfaces.DbContext;

namespace SmartHome.Application.Queries.Devices.Weather.GetWindAggregates
{
    public class GetWindAggregatesQueryHandler : IRequestHandler<GetWindAggregatesQuery, WindAggregatesVm?>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetWindAggregatesQueryHandler(IApplicationDbContext applicationDbContext,
            IDateTimeProvider dateTimeProvider)
        {
            _applicationDbContext = applicationDbContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<WindAggregatesVm?> Handle(GetWindAggregatesQuery request, CancellationToken cancellationToken)
        {
            var currentDate = _dateTimeProvider.GetUtcNow();
            request.WithDefaultValues(currentDate.AddDays(-2), currentDate);

            var query = _applicationDbContext.WeatherStationWind
                .Where(x => x.MeasurementStartTime >= request.From && x.MeasurementEndTime <= request.To);

            var aggregates = await query
                .GroupBy(_ => 1)
                .Select(g => new
                {
                    MaxWindSpeed = g.Max(x => x.MaxWindSpeed),
                    MinWindSpeed = g.Min(x => x.MinWindSpeed),
                    AverageWindSpeed = g.Average(x => x.AverageWindSpeed)
                }).FirstOrDefaultAsync(cancellationToken);

            if(aggregates == null)
            {
                return null;
            }

            var maxWindSpeedTimestamp = await query.Where(x => x.MaxWindSpeed == aggregates.MaxWindSpeed)
                .Select(x => x.MeasurementEndTime).FirstAsync(cancellationToken);

            var minWindSpeedTimestamp = await query.Where(x => x.MinWindSpeed == aggregates.MinWindSpeed)
                .Select(x => x.MeasurementEndTime).FirstAsync(cancellationToken);

            return new WindAggregatesVm
            {
                AverageWindSpeed = aggregates.AverageWindSpeed,
                MaxWindSpeed = aggregates.MaxWindSpeed,
                MinWindSpeed = aggregates.MinWindSpeed,
                MinWindSpeedTimestamp = minWindSpeedTimestamp,
                MaxWindSpeedTimestamp = maxWindSpeedTimestamp
            };
        }
    }
}