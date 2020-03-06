using System.Collections.Generic;
using MediatR;
using SmartHome.Application.Models;

namespace SmartHome.Application.Queries.Devices.Weather.GetTemperatureAggregates
{
    public class GetTemperatureAggregatesQuery : DateRangeRequest, IRequest<TemperatureAggregatesVm>
    {
    }
}