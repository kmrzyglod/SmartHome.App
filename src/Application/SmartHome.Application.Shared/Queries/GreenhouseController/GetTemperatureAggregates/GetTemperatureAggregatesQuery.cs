using MediatR;
using SmartHome.Application.Shared.Models;
using SmartHome.Application.Shared.Queries.SharedModels;

namespace SmartHome.Application.Shared.Queries.GreenhouseController.GetTemperatureAggregates
{
    public class GetTemperatureAggregatesQuery : DateRangeRequest, IRequest<TemperatureAggregatesVm>
    {
    }
}