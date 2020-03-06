using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Interfaces.DateTime;
using SmartHome.Application.Interfaces.DbContext;

namespace SmartHome.Application.Queries.Devices.Weather.GetPrecipitation
{
    public class GetPrecipitationQueryHandler : IRequestHandler<GetPrecipitationQuery, List<PrecipitationVm>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GetPrecipitationQueryHandler(IApplicationDbContext _applicationDbContext,
            IDateTimeProvider _dateTimeProvider)
        {
            this._applicationDbContext = _applicationDbContext;
            this._dateTimeProvider = _dateTimeProvider;
        }

        public async Task<List<PrecipitationVm>> Handle(GetPrecipitationQuery request,
            CancellationToken cancellationToken)
        {
            var currentDate = _dateTimeProvider.GetUtcNow();
            request.WithDefaultValues(currentDate.AddDays(-2), currentDate);
            
            var result =  await _applicationDbContext.WeatherStationPrecipitation
                .Where(x => x.MeasurementStartTime >= request.From && x.MeasurementEndTime <= request.To)
                .Select(x => new
                {
                    DateGroup =  x.MeasurementEndTime.Date,
                    HourGroup = x.MeasurementEndTime.Hour,
                    x.Rain
                })
                .GroupBy(x => new {x.DateGroup, x.HourGroup})
                .Select(g => new PrecipitationVm
                {
                    Timestamp =  DateTime.SpecifyKind(new DateTime(g.Key.DateGroup.Year, g.Key.DateGroup.Month, g.Key.DateGroup.Day, g.Key.HourGroup, 0, 0), DateTimeKind.Utc),
                    RainSum = g.Sum(x => x.Rain)
                })
                .ToListAsync(cancellationToken);



            return result.OrderBy(x => x.Timestamp).ToList();
        }
    }
}