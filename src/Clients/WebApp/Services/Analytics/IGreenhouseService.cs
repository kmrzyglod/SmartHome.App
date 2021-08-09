using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetHumidity;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetInsolation;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetIrrigationData;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetSoilMoisture;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetTemperature;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetTemperatureAggregates;
using SmartHome.Application.Shared.Queries.GreenhouseController.GetWindowsStatus;
using SmartHome.Application.Shared.Queries.SharedModels;

namespace SmartHome.Clients.WebApp.Services.Analytics
{
    public interface IGreenhouseService
    {
        Task<IEnumerable<TemperatureVm>> GetTemperature(GetTemperatureQuery query);
        Task<TemperatureAggregatesVm> GetTemperatureAggregates(GetTemperatureAggregatesQuery query);
        Task<IEnumerable<HumidityVm>> GetHumidity(GetHumidityQuery query);
        Task<IEnumerable<InsolationVm>> GetInsolation(GetInsolationQuery query);
        Task<IEnumerable<SoilMoistureVm>> GetSoilMoisture(GetSoilMoistureQuery query);
        Task<IEnumerable<IrrigationDataVm>> GetIrrigationData(GetIrrigationDataQuery query);
        Task<WindowsStatusVm> GetWindowsStatus(GetWindowsStatusQuery query);
    }
}