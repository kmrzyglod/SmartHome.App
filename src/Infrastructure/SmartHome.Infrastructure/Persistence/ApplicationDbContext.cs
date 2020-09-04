using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartHome.Application.Interfaces.DbContext;
using SmartHome.Domain.Entities.Devices.Greenhouse;
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
        
        public DbSet<WeatherStationAirParameters> WeatherStationAirParameters { get; set; }
        public DbSet<WeatherStationPrecipitation> WeatherStationPrecipitation { get; set; }
        public DbSet<WeatherStationInsolationParameters> WeatherStationInsolationParameters { get; set; }
        public DbSet<WeatherStationWindParameters> WeatherStationWindParameters { get; set; }
        public DbSet<DeviceStatus> DeviceStatuses { get; set; }
        public DbSet<Device> Device { get; set; }
        public DbSet<GreenhouseAirParameters> GreenhouseAirParameters { get; set; }
        public DbSet<GreenhouseInsolationParameters> GreenhouseInsolationParameters { get; set; }
        public DbSet<GreenhouseWindowsStatus> GreenhouseWindowsStatus { get; set; }
        public DbSet<GreenhouseSoilParameters> GreenhouseSoilParameters { get; set; }
        public DbSet<GreenhouseIrrigationHistory> GreenhouseIrrigationHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                        property.SetValueConverter(dateTimeConverter);
                }
            }
            base.OnModelCreating(builder);
        }
    }
}