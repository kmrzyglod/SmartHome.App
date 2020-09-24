using System.Collections.Generic;
using MediatR;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.WeatherStation.GetPrecipitation
{
    public class GetPrecipitationQuery : DateRangeRequest, IRequest<List<PrecipitationVm>>
    {
    }
}