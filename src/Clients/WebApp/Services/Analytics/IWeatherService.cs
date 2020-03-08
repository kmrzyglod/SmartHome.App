using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHome.Application.Shared.Queries.GetHumidity;
using SmartHome.Application.Shared.Queries.GetPrecipitation;
using SmartHome.Application.Shared.Queries.GetPressure;
using SmartHome.Application.Shared.Queries.GetTemperature;
using SmartHome.Application.Shared.Queries.GetWindParameters;

namespace SmartHome.Clients.WebApp.Services.Analytics
{
    public interface IWeatherService
    {
        Task<IEnumerable<TemperatureVm>> GetTemperature(GetTemperatureQuery query);
        Task<IEnumerable<HumidityVm>> GetHumidity(GetHumidityQuery query);
        Task<IEnumerable<PressureVm>> GetPressure(GetPressureQuery query);
        Task<IEnumerable<WindParametersVm>> GetWindParameters(GetWindParametersQuery query);
        Task<IEnumerable<PrecipitationVm>> GetPrecipitation(GetPrecipitationQuery query);
    }
}