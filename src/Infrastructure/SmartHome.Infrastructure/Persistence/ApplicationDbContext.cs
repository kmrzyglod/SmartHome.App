using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Domain.Entities.Devices.Shared;
using SmartHome.Domain.Entities.Devices.WeatherStation;

#pragma warning disable CS8618 
namespace SmartHome.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<Air> WeatherStationAir { get; set; }
        public DbSet<Precipitation> WeatherStationPrecipitation { get; set; }
        public DbSet<Sun> WeatherStationSun { get; set; }
        public DbSet<Wind> WeatherStationWind { get; set; }
        public DbSet<DeviceStatus> DeviceStatuses { get; set; }
        public DbSet<Device> Device { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}