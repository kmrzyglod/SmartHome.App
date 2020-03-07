﻿using System.Collections.Generic;
using MediatR;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.GetPressure
{
    public class GetPressureQuery: DateRangeRequest, IRequest<List<PressureVm>>
    {
    }
}
