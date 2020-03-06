using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Interfaces.DateTime;
using SmartHome.Application.Interfaces.DbContext;

namespace SmartHome.Application.Queries.Devices.Weather.GetPressure
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

            return _applicationDbContext
                .WeatherStationAir
                .Where(x => x.MeasurementStartTime >= request.From && x.MeasurementEndTime <= request.To)
                .Select(x => new PressureVm
                {
                    Timestamp = x.MeasurementEndTime,
                    Pressure = x.Pressure
                })
                .OrderBy(x => x.Timestamp)
                .ToListAsync(cancellationToken);
        }
    }
}