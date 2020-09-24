using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.GreenhouseController.GetSoilMoisture
{
    public class GetSoilMoistureQuery: DateRangeRequest, IRequest<List<SoilMoistureVm>>, IRequest<SoilMoistureVm>
    {
    }
}
