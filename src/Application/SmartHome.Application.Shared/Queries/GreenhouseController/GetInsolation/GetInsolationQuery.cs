using System.Collections.Generic;
using MediatR;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.GreenhouseController.GetInsolation
{
    public class GetInsolationQuery : DateRangeRequest, IRequest<List<InsolationVm>>
    {
    }
}