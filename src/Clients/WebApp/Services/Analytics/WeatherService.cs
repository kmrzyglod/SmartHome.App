using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SmartHome.Application.Shared.Queries.GetHumidity;
using SmartHome.Application.Shared.Queries.GetPressure;
using SmartHome.Application.Shared.Queries.GetTemperature;
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
    }
}
