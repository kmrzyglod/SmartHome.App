using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SmartHome.Clients.WebApp.Services.Analytics;
using SmartHome.Clients.WebApp.Services.Logger;
using SmartHome.Clients.WebApp.Shared.Components.HumidityChart;
using SmartHome.Clients.WebApp.Shared.Components.InsolationChart;
using SmartHome.Clients.WebApp.Shared.Components.IrrigationChart;
using SmartHome.Clients.WebApp.Shared.Components.SoilMoistureChart;
using SmartHome.Clients.WebApp.Shared.Components.TemperatureChart;

namespace SmartHome.Clients.WebApp.Pages.Analytics.Greenhouse
{
    public class GreenhouseViewModel : ComponentBase
    {
        [Inject] protected IGreenhouseService GreenhouseService { get; set; } = null!;

        protected TemperatureChartComponent TemperatureChart { get; set; } = null!;
        protected HumidityChartComponent HumidityChart { get; set; } = null!;
        protected InsolationChartComponent InsolationChart { get; set; } = null!;
        protected IrrigationChartComponent IrrigationChart { get; set; } = null!;
        protected SoilMoistureChartComponent SoilMoistureChart { get; set; } = null!;


        protected override void OnInitialized()
        {
        }

        protected override async Task OnInitializedAsync()
        {
        }


        protected async Task OnTabChange(int index)
        {
            try
            {
                switch (index)
                {
                    case 0:
                        await TemperatureChart.UpdateData();
                        break;
                    case 1:
                        await HumidityChart.UpdateData();
                        break;
                    case 2:
                        await InsolationChart.UpdateData();
                        break;
                    case 3:
                        await IrrigationChart.UpdateData();
                        break;
                    case 4:
                        await SoilMoistureChart.UpdateData();
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Failed to load weather data");
            }
        }
    }
}