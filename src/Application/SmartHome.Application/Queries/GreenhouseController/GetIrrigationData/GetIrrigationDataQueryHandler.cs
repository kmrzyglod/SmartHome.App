using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Application.Shared.Enums;
using SmartHome.Application.Shared.Interfaces.DateTime;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetIrrigationData;

namespace SmartHome.Application.Queries.GreenhouseController.GetIrrigationData
{
    public class GetIrrigationDataQueryHandler : IRequestHandler<GetIrrigationDataQuery, List<IrrigationDataVm>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetIrrigationDataQueryHandler(IApplicationDbContext applicationDbContext,
            IDateTimeProvider dateTimeProvider)
        {
            _applicationDbContext = applicationDbContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public Task<List<IrrigationDataVm>> Handle(GetIrrigationDataQuery request, CancellationToken cancellationToken)
        {
            var currentDate = _dateTimeProvider.GetUtcNow();
            request.WithDefaultValues(currentDate.AddDays(-2), currentDate);
            int granulation = (int) (request.Granulation ?? default);
            var fromDate = request.From!.Value;

            return _applicationDbContext.GreenhouseIrrigationHistory
                .AsNoTracking()
                .Where(x => x.MeasurementStartTime >= request.From && x.MeasurementEndTime <= request.To)
                .Select(x => new
                {
                    TimestampGroup = granulation == (int) DateRangeGranulation.Year
                        ? EF.Functions.DateDiffYear(fromDate, x.MeasurementEndTime)
                        : granulation == (int) DateRangeGranulation.Month
                            ? EF.Functions.DateDiffMonth(fromDate, x.MeasurementEndTime)
                            : EF.Functions.DateDiffSecond(fromDate, x.MeasurementEndTime) / granulation,
                    x.AverageWaterFlow,
                    x.MaxWaterFlow,
                    x.MinWaterFlow,
                    x.TotalWaterVolume
                }) 
                .GroupBy(x => x.TimestampGroup)
                .OrderBy(x => x.Key)
                .Select(g => new IrrigationDataVm
                {
                    Timestamp = DateTime.SpecifyKind(
                        granulation == (int) DateRangeGranulation.Year
                            ? fromDate.AddYears(g.Key)
                            : granulation == (int) DateRangeGranulation.Month
                                ? fromDate.AddMonths(g.Key)
                                : fromDate.AddSeconds(g.Key * granulation),
                        DateTimeKind.Utc),
                    AverageWaterFlow = g.Average(x => x.AverageWaterFlow),
                    MaxWaterFlow = g.Max(x => x.MaxWaterFlow),
                    MinWaterFlow = g.Min(x => x.MinWaterFlow),
                    TotalWaterVolume = g.Sum(x => x.TotalWaterVolume)
                })
                .ToListAsync(cancellationToken);
        }
    }
}