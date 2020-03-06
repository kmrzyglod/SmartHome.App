using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using SmartHome.Application.Models;

namespace SmartHome.Application.Queries.Devices.Weather.GetPressure
{
    public class GetPressureQuery: DateRangeRequest, IRequest<List<PressureVm>>
    {
    }
}
