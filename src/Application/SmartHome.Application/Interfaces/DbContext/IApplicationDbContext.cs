using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHome.Domain.Entities.Devices.Greenhouse;
using SmartHome.Domain.Entities.Devices.Shared;
using SmartHome.Domain.Entities.Devices.WeatherStation;

namespace SmartHome.Application.Interfaces.DbContext
{
    public interface IApplicationDbContext: IDisposable
    {
        //Weather station
        DbSet<WeatherStationAirParameters> WeatherStationAirParameters { get; set; }
        DbSet<WeatherStationPrecipitation> WeatherStationPrecipitation { get; set; }
        DbSet<WeatherStationInsolationParameters> WeatherStationInsolationParameters { get; set; }
        DbSet<WeatherStationWindParameters> WeatherStationWindParameters { get; set; }
        
        //general devices
        DbSet<DeviceStatus> DeviceStatuses { get; set; }
        DbSet<Device> Device { get; set; }

        //Greenhouse controller & indows controller
        DbSet<GreenhouseAirParameters> GreenhouseAirParameters { get; set; }
        DbSet<GreenhouseInsolationParameters> GreenhouseInsolationParameters { get; set; }
        DbSet<GreenhouseSoilParameters> GreenhouseSoilParameters { get; set; }
        DbSet<GreenhouseIrrigationHistory> GreenhouseIrrigationHistory { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}