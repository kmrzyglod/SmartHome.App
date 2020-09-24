using MediatR;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.WeatherStation.GetWindAggregates
{
    public class GetWindAggregatesQuery : DateRangeRequest, IRequest<WindAggregatesVm>
    {
    }
}