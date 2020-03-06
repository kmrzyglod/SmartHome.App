using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Interfaces.DateTime;
using SmartHome.Application.Interfaces.DbContext;

namespace SmartHome.Application.Queries.Devices.Weather.GetTemperature
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
            return _applicationDbContext.WeatherStationAir
                .AsNoTracking()
                .Where(x => x.MeasurementStartTime >= request.From && x.MeasurementEndTime <= request.To)
                .Select(x => new TemperatureVm
                {
                    Temperature = x.Temperature,
                    Timestamp = x.MeasurementEndTime
                })
                .OrderBy(x => x.Timestamp)
                .ToListAsync(cancellationToken);
        }
    }
}