using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetHumidity;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetInsolation;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetIrrigationData;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetSoilMoisture;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetTemperature;

namespace SmartHome.Clients.WebApp.Services.Analytics
{
    public interface IGreenhouseService
    {
        Task<IEnumerable<TemperatureVm>> GetTemperature(GetTemperatureQuery query);
        Task<IEnumerable<HumidityVm>> GetHumidity(GetHumidityQuery query);
        Task<IEnumerable<InsolationVm>> GetInsolation(GetInsolationQuery query);
        Task<IEnumerable<SoilMoistureVm>> GetSoilMoisture(GetSoilMoistureQuery query);
        Task<IEnumerable<IrrigationDataVm>> GetIrrigationData(GetIrrigationDataQuery query);
      
    }
}