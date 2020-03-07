using MediatR;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.GetWindAggregates
{
    public class GetWindAggregatesQuery : DateRangeRequest, IRequest<WindAggregatesVm>
    {
    }
}