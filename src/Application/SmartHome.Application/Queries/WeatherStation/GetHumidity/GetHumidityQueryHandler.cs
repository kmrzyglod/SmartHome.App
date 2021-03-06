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
using SmartHome.Application.Shared.Queries.WeatherStation.GetHumidity;

namespace SmartHome.Application.Queries.WeatherStation.GetHumidity
{
    public class GetHumidityQueryHandler : IRequestHandler<GetHumidityQuery, List<HumidityVm>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetHumidityQueryHandler(IApplicationDbContext applicationDbContext, IDateTimeProvider dateTimeProvider)
        {
            _applicationDbContext = applicationDbContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public Task<List<HumidityVm>> Handle(GetHumidityQuery request, CancellationToken cancellationToken)
        {
            var currentDate = _dateTimeProvider.GetUtcNow();
            request.WithDefaultValues(currentDate.AddDays(-2), currentDate);
            int granulation = (int) (request.Granulation ?? default);
            Debug.Assert(request.From != null, "request.From != null");
            var fromDate = request.From.Value;
            return _applicationDbContext.WeatherStationAirParameters
                .AsNoTracking()
                .Where(x => x.MeasurementStartTime >= request.From && x.MeasurementEndTime <= request.To)
                .Select(x => new
                {
                    TimestampGroup = granulation == (int) DateRangeGranulation.Year
                        ? EF.Functions.DateDiffYear(fromDate, x.MeasurementEndTime)
                        : granulation == (int) DateRangeGranulation.Month
                            ? EF.Functions.DateDiffMonth(fromDate, x.MeasurementEndTime)
                            : EF.Functions.DateDiffSecond(fromDate, x.MeasurementEndTime) / granulation,
                    x.Humidity,
                })
                .GroupBy(x => x.TimestampGroup)
                .OrderBy(x => x.Key)
                .Select(g => new HumidityVm()
                {
                    Timestamp = DateTime.SpecifyKind(
                        granulation == (int) DateRangeGranulation.Year
                            ? fromDate.AddYears(g.Key)
                            : granulation == (int) DateRangeGranulation.Month
                                ? fromDate.AddMonths(g.Key)
                                : fromDate.AddSeconds(g.Key * granulation),
                        DateTimeKind.Utc),
                    Humidity = Math.Round(g.Average(x => x.Humidity), 2),
                    MaxHumidity = Math.Round(g.Max(x => x.Humidity), 2),
                    MinHumidity = Math.Round(g.Min(x => x.Humidity), 2)
                })
                .ToListAsync(cancellationToken);
        }
    }
}