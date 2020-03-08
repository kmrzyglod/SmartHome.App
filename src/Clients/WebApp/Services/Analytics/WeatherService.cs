using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SmartHome.Application.Shared.Queries.GetHumidity;
using SmartHome.Application.Shared.Queries.GetPrecipitation;
using SmartHome.Application.Shared.Queries.GetPressure;
using SmartHome.Application.Shared.Queries.GetTemperature;
using SmartHome.Application.Shared.Queries.GetWindParameters;
using SmartHome.Clients.WebApp.Services.Shared.ApiClient;

namespace SmartHome.Clients.WebApp.Services.Analytics
{
    public class WeatherService : IWeatherService
    {
        private readonly IApiClient _apiClient;

        public WeatherService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public Task<IEnumerable<TemperatureVm>> GetTemperature(GetTemperatureQuery query)
        {
            return _apiClient.Get<GetTemperatureQuery, IEnumerable<TemperatureVm>>("WeatherData/temperature", query);
        }

        public Task<IEnumerable<HumidityVm>> GetHumidity(GetHumidityQuery query)
        {
            return _apiClient.Get<GetHumidityQuery, IEnumerable<HumidityVm>>("WeatherData/humidity", query);
        }

        public Task<IEnumerable<PressureVm>> GetPressure(GetPressureQuery query)
        {
            return _apiClient.Get<GetPressureQuery, IEnumerable<PressureVm>>("WeatherData/pressure", query);
        }

        public Task<IEnumerable<WindParametersVm>> GetWindParameters(GetWindParametersQuery query)
        {
            return _apiClient.Get<GetWindParametersQuery, IEnumerable<WindParametersVm>>("WeatherData/wind", query);
        }

        public Task<IEnumerable<PrecipitationVm>> GetPrecipitation(GetPrecipitationQuery query)
        {
            return _apiClient.Get<GetPrecipitationQuery, IEnumerable<PrecipitationVm>>("WeatherData/precipitation", query);
        }
    }
}
