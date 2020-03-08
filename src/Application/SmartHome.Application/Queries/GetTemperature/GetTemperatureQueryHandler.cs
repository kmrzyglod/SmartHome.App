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
using SmartHome.Application.Shared.Queries.GetTemperature;

namespace SmartHome.Application.Queries.GetTemperature
{
    public class GetTemperatureQueryHandler : IRequestHandler<GetTemperatureQuery, List<TemperatureVm>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetTemperatureQueryHandler(IApplicationDbContext _applicationDbContext,
            IDateTimeProvider _dateTimeProvider)
        {
            this._applicationDbContext = _applicationDbContext;
            this._dateTimeProvider = _dateTimeProvider;
        }

        public Task<List<TemperatureVm>> Handle(GetTemperatureQuery request,
            CancellationToken cancellationToken)
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
                    x.Temperature
                })
                .GroupBy(x => x.TimestampGroup)
                .Select(g => new TemperatureVm
                {
                    Timestamp = DateTime.SpecifyKind(
                        granulation == (int) DateRangeGranulation.Year
                            ? fromDate.AddYears(g.Key)
                            : granulation == (int) DateRangeGranulation.Month
                                ? fromDate.AddMonths(g.Key)
                                : fromDate.AddSeconds(g.Key * granulation),
                        DateTimeKind.Utc),
                    Temperature = Math.Round(g.Average(x => x.Temperature), 2),
                    MaxTemperature = Math.Round(g.Max(x => x.Temperature), 2),
                    MinTemperature = Math.Round(g.Min(x => x.Temperature), 2)
                })
                .ToListAsync(cancellationToken);
        }
    }
}