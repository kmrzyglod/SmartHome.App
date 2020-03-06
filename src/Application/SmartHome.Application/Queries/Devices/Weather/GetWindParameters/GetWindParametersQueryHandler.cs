using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Interfaces.DateTime;
using SmartHome.Application.Interfaces.DbContext;

namespace SmartHome.Application.Queries.Devices.Weather.GetWindParameters
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

        public Task<List<WindParametersVm>> Handle(GetWindParametersQuery request, CancellationToken cancellationToken)
        {
            var currentDate = _dateTimeProvider.GetUtcNow();
            request.WithDefaultValues(currentDate.AddDays(-2), currentDate);
            return _applicationDbContext
                .WeatherStationWind
                .Where(x => x.MeasurementStartTime >= request.From && x.MeasurementEndTime <= request.To)
                .Select(x => new WindParametersVm
                {
                    AverageWindSpeed = x.AverageWindSpeed,
                    MaxWindSpeed = x.MaxWindSpeed,
                    MinWindSpeed = x.MinWindSpeed,
                    MostFrequentWindDirection = x.MostFrequentWindDirection,
                    Timestamp = x.MeasurementEndTime
                })
                .OrderBy(x => x.Timestamp)
                .ToListAsync(cancellationToken);
        }
    }
}