using System.Collections.Generic;
using MediatR;
using SmartHome.Application.Shared.Models;

namespace SmartHome.Application.Shared.Queries.WeatherStation.GetHumidity
{
    public class GetHumidityQuery:  DateRangeRequest, IRequest<List<HumidityVm>> 
    {
    }
}
