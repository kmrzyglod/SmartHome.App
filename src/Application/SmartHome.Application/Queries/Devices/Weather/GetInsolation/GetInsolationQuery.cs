using System.Collections.Generic;
using MediatR;
using SmartHome.Application.Models;

namespace SmartHome.Application.Queries.Devices.Weather.GetInsolation
{
    public class GetInsolationQuery : DateRangeRequest, IRequest<List<InsolationVm>>
    {
    }
}