using MediatR;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.GetTemperatureAggregates
{
    public class GetTemperatureAggregatesQuery : DateRangeRequest, IRequest<TemperatureAggregatesVm>
    {
    }
}