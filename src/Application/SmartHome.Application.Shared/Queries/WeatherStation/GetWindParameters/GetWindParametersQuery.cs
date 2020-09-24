using System.Collections.Generic;
using MediatR;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.WeatherStation.GetWindParameters
{
    public class GetWindParametersQuery: DateRangeRequest, IRequest<List<WindParametersVm>>
    {

    }
}