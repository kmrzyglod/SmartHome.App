using System.Collections.Generic;
using MediatR;
using SmartHome.Application.Shared.Models;
using SmartHome.Application.Shared.Queries.SharedModels;

namespace SmartHome.Application.Shared.Queries.WeatherStation.GetInsolation
{
    public class GetInsolationQuery : DateRangeRequest, IRequest<List<InsolationVm>>
    {
    }
}