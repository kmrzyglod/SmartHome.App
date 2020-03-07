using System.Collections.Generic;
using MediatR;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.GetPrecipitation
{
    public class GetPrecipitationQuery : DateRangeRequest, IRequest<List<PrecipitationVm>>
    {
    }
}