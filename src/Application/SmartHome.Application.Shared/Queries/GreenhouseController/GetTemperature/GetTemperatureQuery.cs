using System.Collections.Generic;
using MediatR;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.GreenhouseController.GetTemperature
{
    public class GetTemperatureQuery : DateRangeRequest, IRequest<List<TemperatureVm>>
    {

    }
}