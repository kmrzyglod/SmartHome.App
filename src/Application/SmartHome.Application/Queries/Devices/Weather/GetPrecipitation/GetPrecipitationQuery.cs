using System.Collections.Generic;
using MediatR;
using SmartHome.Application.Models;

namespace SmartHome.Application.Queries.Devices.Weather.GetPrecipitation
{
    public class GetPrecipitationQuery : DateRangeRequest, IRequest<List<PrecipitationVm>>
    {
    }
}