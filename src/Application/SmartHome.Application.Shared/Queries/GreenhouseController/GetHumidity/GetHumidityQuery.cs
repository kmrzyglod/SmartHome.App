﻿using System.Collections.Generic;
using MediatR;
using SmartHome.Application.Shared.Models;
using SmartHome.Application.Shared.Queries.SharedModels;

namespace SmartHome.Application.Shared.Queries.GreenhouseController.GetHumidity
{
    public class GetHumidityQuery:  DateRangeRequest, IRequest<List<HumidityVm>> 
    {
    }
}
