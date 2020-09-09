using System.Collections.Generic;
using MediatR;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.WeatherStation.GetTemperature
{
    public class GetTemperatureQuery : DateRangeRequest, IRequest<List<TemperatureVm>>
    {

    }
}