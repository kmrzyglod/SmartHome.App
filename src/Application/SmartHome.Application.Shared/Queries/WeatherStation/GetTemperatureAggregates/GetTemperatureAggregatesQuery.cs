using MediatR;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.WeatherStation.GetTemperatureAggregates
{
    public class GetTemperatureAggregatesQuery : DateRangeRequest, IRequest<TemperatureAggregatesVm>
    {
    }
}