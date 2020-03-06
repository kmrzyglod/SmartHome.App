using System.Collections.Generic;
using MediatR;
using SmartHome.Application.Models;

namespace SmartHome.Application.Queries.Devices.Weather.GetWindParameters
{
    public class GetWindParametersQuery: DateRangeRequest, IRequest<List<WindParametersVm>>
    {

    }
}