using MediatR;
using SmartHome.Application.Models;

namespace SmartHome.Application.Queries.Devices.Weather.GetWindAggregates
{
    public class GetWindAggregatesQuery : DateRangeRequest, IRequest<WindAggregatesVm>
    {
    }
}