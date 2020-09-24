using System.Collections.Generic;
using MediatR;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.GreenhouseController.GetIrrigationData
{
    public class GetIrrigationDataQuery: DateRangeRequest, IRequest<List<IrrigationDataVm>>, IRequest<IrrigationDataVm>
    {
    }
}
