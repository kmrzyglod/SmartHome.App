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
using SmartHome.Application.Shared.Queries.GetPressure;

namespace SmartHome.Application.Queries.GetPressure
{
    public class GetPressureQueryHandler : IRequestHandler<GetPressureQuery, List<PressureVm>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetPressureQueryHandler(IApplicationDbContext applicationDbContext, IDateTimeProvider dateTimeProvider)
        {
            _applicationDbContext = applicationDbContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public Task<List<PressureVm>> Handle(GetPressureQuery request, CancellationToken cancellationToken)
        {
            var currentDate = _dateTimeProvider.GetUtcNow();
            request.WithDefaultValues(currentDate.AddDays(-2), currentDate);
            int granulation = (int) (request.Granulation ?? default);
            Debug.Assert(request.From != null, "request.From != null");
            var fromDate = request.From.Value;

            return _applicationDbContext.WeatherStationAir
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
                    x.Pressure
                })
                .GroupBy(x => x.TimestampGroup)
                .Select(g => new PressureVm
                {
                    Timestamp = DateTime.SpecifyKind(
                        granulation == (int) DateRangeGranulation.Year
                            ? fromDate.AddYears(g.Key)
                            : granulation == (int) DateRangeGranulation.Month
                                ? fromDate.AddMonths(g.Key)
                                : fromDate.AddSeconds(g.Key * granulation),
                        DateTimeKind.Utc),
                    Pressure = Math.Round(g.Average(x => x.Pressure), 2),
                    MaxPressure = Math.Round(g.Max(x => x.Pressure), 2),
                    MinPressure = Math.Round(g.Min(x => x.Pressure), 2)
                })
                .ToListAsync(cancellationToken);
        }
    }
}