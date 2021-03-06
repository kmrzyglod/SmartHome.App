﻿using System;
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
using SmartHome.Application.Shared.Queries.SharedModels;
using SmartHome.Application.Shared.Queries.WeatherStation.GetInsolation;

namespace SmartHome.Application.Queries.WeatherStation.GetInsolation
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
            int granulation = (int) (request.Granulation ?? default);
            Debug.Assert(request.From != null, "request.From != null");
            var fromDate = request.From.Value;

            return _applicationDbContext.WeatherStationInsolationParameters
                .AsNoTracking()
                .Where(x => x.MeasurementStartTime >= request.From && x.MeasurementEndTime <= request.To)
                .Select(x => new
                {
                    TimestampGroup = granulation == (int) DateRangeGranulation.Year
                        ? EF.Functions.DateDiffYear(fromDate, x.MeasurementEndTime)
                        : granulation == (int) DateRangeGranulation.Month
                            ? EF.Functions.DateDiffMonth(fromDate, x.MeasurementEndTime)
                            : EF.Functions.DateDiffSecond(fromDate, x.MeasurementEndTime) / granulation,
                    x.LightLevelInLux,
                })
                .GroupBy(x => x.TimestampGroup)
                .OrderBy(x => x.Key)
                .Select(g => new InsolationVm()
                {
                    Timestamp = DateTime.SpecifyKind(
                        granulation == (int) DateRangeGranulation.Year
                            ? fromDate.AddYears(g.Key)
                            : granulation == (int) DateRangeGranulation.Month
                                ? fromDate.AddMonths(g.Key)
                                : fromDate.AddSeconds(g.Key * granulation),
                        DateTimeKind.Utc),
                    LightLevel = Math.Round(g.Average(x => x.LightLevelInLux), 2),
                    MaxLightLevel = Math.Round(g.Max(x => x.LightLevelInLux), 2),
                    MinLightLevel = Math.Round(g.Min(x => x.LightLevelInLux), 2)
                })
                .ToListAsync(cancellationToken);
        }
    }
}