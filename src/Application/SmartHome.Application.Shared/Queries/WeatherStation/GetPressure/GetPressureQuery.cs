﻿using System.Collections.Generic;
using MediatR;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.WeatherStation.GetPressure
{
    public class GetPressureQuery: DateRangeRequest, IRequest<List<PressureVm>>
    {
    }
}
