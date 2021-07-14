using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetTemperatureAggregates;
using SmartHome.Application.Shared.Queries.SharedModels;
using SmartHome.Application.Shared.Queries.WeatherStation.GetHumidity;
using SmartHome.Application.Shared.Queries.WeatherStation.GetPrecipitation;
using SmartHome.Application.Shared.Queries.WeatherStation.GetPressure;
using SmartHome.Application.Shared.Queries.WeatherStation.GetTemperature;
using SmartHome.Application.Shared.Queries.WeatherStation.GetWindAggregates;
using SmartHome.Application.Shared.Queries.WeatherStation.GetWindParameters;
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

        public Task<TemperatureAggregatesVm> GetTemperatureAggregates(GetTemperatureAggregatesQuery query)
        {
            return _apiClient.Get<GetTemperatureAggregatesQuery, TemperatureAggregatesVm>("WeatherData/temperature/aggregates", query);
        }

        public Task<WindAggregatesVm> GetWindAggregates(GetWindAggregatesQuery query)
        {
            return _apiClient.Get<GetWindAggregatesQuery, WindAggregatesVm>("WeatherData/wind/aggregates", query);
        }

        public Task<IEnumerable<PrecipitationVm>> GetPrecipitation(GetPrecipitationQuery query)
        {
            return _apiClient.Get<GetPrecipitationQuery, IEnumerable<PrecipitationVm>>("WeatherData/precipitation", query);
        }
    }
}
