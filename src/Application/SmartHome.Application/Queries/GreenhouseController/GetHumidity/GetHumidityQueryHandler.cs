﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetHumidity;
using SmartHome.Application.Shared.Queries.SharedModels;

namespace SmartHome.Application.Queries.GreenhouseController.GetHumidity
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
            var fromDate = request.From!.Value;

            return _applicationDbContext.GreenhouseAirParameters
                .AsNoTracking()
                .Where(x => x.Timestamp >= request.From && x.Timestamp <= request.To)
                .Select(x => new
                {
                    TimestampGroup = granulation == (int) DateRangeGranulation.Year
                        ? EF.Functions.DateDiffYear(fromDate, x.Timestamp)
                        : granulation == (int) DateRangeGranulation.Month
                            ? EF.Functions.DateDiffMonth(fromDate, x.Timestamp)
                            : EF.Functions.DateDiffSecond(fromDate, x.Timestamp) / granulation,
                    x.Humidity
                })
                .GroupBy(x => x.TimestampGroup)
                .OrderBy(x => x.Key)
                .Select(g => new HumidityVm
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