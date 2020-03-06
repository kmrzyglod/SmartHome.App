using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Interfaces.DateTime;
using SmartHome.Application.Interfaces.DbContext;

namespace SmartHome.Application.Queries.Devices.Weather.GetTemperatureAggregates
{
    public class
        GetTemperatureAggregatesQueryHandler : IRequestHandler<GetTemperatureAggregatesQuery, TemperatureAggregatesVm?>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetTemperatureAggregatesQueryHandler(IApplicationDbContext applicationDbContext, IDateTimeProvider dateTimeProvider)
        {
            _applicationDbContext = applicationDbContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<TemperatureAggregatesVm?> Handle(GetTemperatureAggregatesQuery request,
            CancellationToken cancellationToken)
        {
            var currentDate = _dateTimeProvider.GetUtcNow();
            request.WithDefaultValues(currentDate.AddDays(-2), currentDate);
            var query = _applicationDbContext.WeatherStationAir
                .Where(x => x.MeasurementStartTime >= request.From && x.MeasurementEndTime <= request.To);

            var aggregates = await query
                .GroupBy(_ => 1)
                .Select(g => new
                {
                    MaxTemperature = g.Max(x => x.Temperature),
                    MinTemperature = g.Min(x => x.Temperature),
                    AverageTemperature = g.Average(x => x.Temperature)
                }).FirstOrDefaultAsync(cancellationToken);

            if(aggregates == null)
            {
                return null;
            }

            var maxTempTimestamp = await query.Where(x => x.Temperature == aggregates.MaxTemperature)
                .Select(x => x.MeasurementEndTime).FirstAsync(cancellationToken);
            
            var minTempTimestamp = await query.Where(x => x.Temperature == aggregates.MinTemperature)
                .Select(x => x.MeasurementEndTime).FirstAsync(cancellationToken);

            return new TemperatureAggregatesVm
            {
                MinTemperature = aggregates.MinTemperature,
                MaxTemperature = aggregates.MaxTemperature,
                MaxTemperatureTimestamp = maxTempTimestamp,
                MinTemperatureTimestamp = minTempTimestamp
            };
        }
    }
}