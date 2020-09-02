using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartHome.Domain.Entities.Devices.Shared;
using SmartHome.Domain.Entities.Devices.WeatherStation;

namespace SmartHome.Application.Interfaces.DbContext
{
    public interface IApplicationDbContext: IDisposable
    {
        //Weather station
        DbSet<Air> WeatherStationAir { get; set; }
        DbSet<Precipitation> WeatherStationPrecipitation { get; set; }
        DbSet<Sun> WeatherStationSun { get; set; }
        DbSet<Wind> WeatherStationWind { get; set; }
        
        //general devices
        DbSet<DeviceStatus> DeviceStatuses { get; set; }
        DbSet<Device> Device { get; set; }


        //Windows controller

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}