using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.Queries.WeatherStation.GetWindParameters;

namespace SmartHome.Application.Queries.WeatherStation.GetWindParameters
{
    public class GetWindParametersQueryHandler : IRequestHandler<GetWindParametersQuery, List<WindParametersVm>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetWindParametersQueryHandler(IApplicationDbContext applicationDbContext,
            IDateTimeProvider dateTimeProvider)
        {
            _applicationDbContext = applicationDbContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<List<WindParametersVm>> Handle(GetWindParametersQuery request,
            CancellationToken cancellationToken)
        {
            var currentDate = _dateTimeProvider.GetUtcNow();
            request.WithDefaultValues(currentDate.AddDays(-2), currentDate);
            int granulation = (int) (request.Granulation ?? default);
            Debug.Assert(request.From != null, "request.From != null");
            var fromDate = request.From.Value;

            var result = await _applicationDbContext.WeatherStationWindParameters
                .AsNoTracking()
                .Where(x => x.MeasurementStartTime >= request.From && x.MeasurementEndTime <= request.To)
                .OrderBy(x => x.MeasurementEndTime)
                .Select(x => new
                {
                    TimestampGroup = granulation == (int) DateRangeGranulation.Year
                        ? EF.Functions.DateDiffYear(fromDate, x.MeasurementEndTime)
                        : granulation == (int) DateRangeGranulation.Month
                            ? EF.Functions.DateDiffMonth(fromDate, x.MeasurementEndTime)
                            : EF.Functions.DateDiffSecond(fromDate, x.MeasurementEndTime) / granulation,
                    x.AverageWindSpeed,
                    x.MaxWindSpeed,
                    x.MinWindSpeed,
                    x.MostFrequentWindDirection
                })
                .ToListAsync(cancellationToken);

            //EF Core 3.1 cannot do nested grouping on DB side so we need do this in-memory ...
            return result.GroupBy(x => x.TimestampGroup)
                .Select(g => new WindParametersVm
                {
                    Timestamp = DateTime.SpecifyKind(
                        granulation == (int) DateRangeGranulation.Year
                            ? fromDate.AddYears(g.Key)
                            : granulation == (int) DateRangeGranulation.Month
                                ? fromDate.AddMonths(g.Key)
                                : fromDate.AddSeconds(g.Key * granulation),
                        DateTimeKind.Utc),
                    AverageWindSpeed = Math.Round(g.Average(x => x.AverageWindSpeed), 2),
                    MaxWindSpeed = Math.Round(g.Max(x => x.MaxWindSpeed), 2),
                    MinWindSpeed = Math.Round(g.Min(x => x.MinWindSpeed), 2),
                    MostFrequentWindDirection = g
                        .GroupBy(x => x.MostFrequentWindDirection)
                        .Select(f => new {Direction = f.Key, Count = f.Count()})
                        .OrderByDescending(f => f.Count).First().Direction
                })
                .ToList();
        }
    }
}