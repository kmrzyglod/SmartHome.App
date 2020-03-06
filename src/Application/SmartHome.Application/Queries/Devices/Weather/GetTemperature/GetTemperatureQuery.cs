using System.Collections.Generic;
using MediatR;
using SmartHome.Application.Models;

namespace SmartHome.Application.Queries.Devices.Weather.GetTemperature
{
    public class GetTemperatureQuery : DateRangeRequest, IRequest<List<TemperatureVm>>
    {
    }
}