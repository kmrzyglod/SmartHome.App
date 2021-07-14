using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetTemperatureAggregates;
using SmartHome.Application.Shared.Queries.SharedModels;
using SmartHome.Application.Shared.Queries.WeatherStation.GetHumidity;
using SmartHome.Application.Shared.Queries.WeatherStation.GetPrecipitation;
using SmartHome.Application.Shared.Queries.WeatherStation.GetPressure;
using SmartHome.Application.Shared.Queries.WeatherStation.GetTemperature;
using SmartHome.Application.Shared.Queries.WeatherStation.GetWindAggregates;
using SmartHome.Application.Shared.Queries.WeatherStation.GetWindParameters;

namespace SmartHome.Clients.WebApp.Services.Analytics
{
    public interface IWeatherService
    {
        Task<IEnumerable<TemperatureVm>> GetTemperature(GetTemperatureQuery query);
        Task<IEnumerable<HumidityVm>> GetHumidity(GetHumidityQuery query);
        Task<IEnumerable<PressureVm>> GetPressure(GetPressureQuery query);
        Task<IEnumerable<WindParametersVm>> GetWindParameters(GetWindParametersQuery query);
        Task<TemperatureAggregatesVm> GetTemperatureAggregates(GetTemperatureAggregatesQuery query);
        Task<WindAggregatesVm> GetWindAggregates(GetWindAggregatesQuery query);
        Task<IEnumerable<PrecipitationVm>> GetPrecipitation(GetPrecipitationQuery query);
    }
}