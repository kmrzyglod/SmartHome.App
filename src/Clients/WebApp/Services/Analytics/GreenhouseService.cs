using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetHumidity;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetInsolation;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetIrrigationData;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetSoilMoisture;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetTemperature;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetTemperatureAggregates;
using SmartHome.Application.Shared.Queries.SharedModels;
using SmartHome.Clients.WebApp.Services.Shared.ApiClient;

namespace SmartHome.Clients.WebApp.Services.Analytics
{
    public class GreenhouseService : IGreenhouseService
    {
        private readonly IApiClient _apiClient;

        public GreenhouseService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public Task<IEnumerable<TemperatureVm>> GetTemperature(GetTemperatureQuery query)
        {
            return _apiClient.Get<GetTemperatureQuery, IEnumerable<TemperatureVm>>("GreenhouseData/temperature", query);
        }

        public Task<TemperatureAggregatesVm> GetTemperatureAggregates(GetTemperatureAggregatesQuery query)
        {
            return _apiClient.Get<GetTemperatureAggregatesQuery, TemperatureAggregatesVm>("GreenhouseData/temperature/aggregates", query);
        }

        public Task<IEnumerable<HumidityVm>> GetHumidity(GetHumidityQuery query)
        {
            return _apiClient.Get<GetHumidityQuery, IEnumerable<HumidityVm>>("GreenhouseData/humidity", query);
        }

        public Task<IEnumerable<InsolationVm>> GetInsolation(GetInsolationQuery query)
        {
            return _apiClient.Get<GetInsolationQuery, IEnumerable<InsolationVm>>("GreenhouseData/insolation", query);
        }

        public Task<IEnumerable<SoilMoistureVm>> GetSoilMoisture(GetSoilMoistureQuery query)
        {
            return _apiClient.Get<GetSoilMoistureQuery, IEnumerable<SoilMoistureVm>>("GreenhouseData/soil-moisture", query);
        }

        public Task<IEnumerable<IrrigationDataVm>> GetIrrigationData(GetIrrigationDataQuery query)
        {
            return _apiClient.Get<GetIrrigationDataQuery, IEnumerable<IrrigationDataVm>>("GreenhouseData/irrigation-data", query);
        }
    }
}
