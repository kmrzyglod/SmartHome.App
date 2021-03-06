﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartHome.Infrastructure.Persistence;

namespace SmartHome.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SmartHome.Domain.Entities.Devices.Greenhouse.GreenhouseAirParameters", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Humidity")
                        .HasColumnType("float");

                    b.Property<double>("Pressure")
                        .HasColumnType("float");

                    b.Property<double>("Temperature")
                        .HasColumnType("float");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("GreenhouseAirParameters");
                });

            modelBuilder.Entity("SmartHome.Domain.Entities.Devices.Greenhouse.GreenhouseInsolationParameters", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("LightLevelInLux")
                        .HasColumnType("float");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("GreenhouseInsolationParameters");
                });

            modelBuilder.Entity("SmartHome.Domain.Entities.Devices.Greenhouse.GreenhouseIrrigationHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("AverageWaterFlow")
                        .HasColumnType("float");

                    b.Property<double>("MaxWaterFlow")
                        .HasColumnType("float");

                    b.Property<DateTime>("MeasurementEndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("MeasurementStartTime")
                        .HasColumnType("datetime2");

                    b.Property<double>("MinWaterFlow")
                        .HasColumnType("float");

                    b.Property<double>("TotalWaterVolume")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("GreenhouseIrrigationHistory");
                });

            modelBuilder.Entity("SmartHome.Domain.Entities.Devices.Greenhouse.GreenhouseSoilParameters", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("SoilMoisture")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("GreenhouseSoilParameters");
                });

            modelBuilder.Entity("SmartHome.Domain.Entities.Devices.Shared.Device", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DeviceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOnline")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastStatusUpdate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId")
                        .IsUnique();

                    b.ToTable("Device");
                });

            modelBuilder.Entity("SmartHome.Domain.Entities.Devices.Shared.DeviceStatus", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("DeviceId")
                        .HasColumnType("bigint");

                    b.Property<long?>("FreeHeapMemory")
                        .HasColumnType("bigint");

                    b.Property<string>("GatewayIp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ip")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Rssi")
                        .HasColumnType("float");

                    b.Property<string>("Ssid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.ToTable("DeviceStatuses");
                });

            modelBuilder.Entity("SmartHome.Domain.Entities.Devices.WeatherStation.WeatherStationAirParameters", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Humidity")
                        .HasColumnType("float");

                    b.Property<DateTime>("MeasurementEndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("MeasurementStartTime")
                        .HasColumnType("datetime2");

                    b.Property<double>("Pressure")
                        .HasColumnType("float");

                    b.Property<double>("Temperature")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("WeatherStationAirParameters");
                });

            modelBuilder.Entity("SmartHome.Domain.Entities.Devices.WeatherStation.WeatherStationInsolationParameters", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("LightLevelInLux")
                        .HasColumnType("float");

                    b.Property<DateTime>("MeasurementEndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("MeasurementStartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("WeatherStationInsolationParameters");
                });

            modelBuilder.Entity("SmartHome.Domain.Entities.Devices.WeatherStation.WeatherStationPrecipitation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("MeasurementEndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("MeasurementStartTime")
                        .HasColumnType("datetime2");

                    b.Property<double>("Rain")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("WeatherStationPrecipitation");
                });

            modelBuilder.Entity("SmartHome.Domain.Entities.Devices.WeatherStation.WeatherStationWindParameters", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("AverageWindSpeed")
                        .HasColumnType("float");

                    b.Property<int>("CurrentWindDirection")
                        .HasColumnType("int");

                    b.Property<double>("MaxWindSpeed")
                        .HasColumnType("float");

                    b.Property<DateTime>("MeasurementEndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("MeasurementStartTime")
                        .HasColumnType("datetime2");

                    b.Property<double>("MinWindSpeed")
                        .HasColumnType("float");

                    b.Property<int>("MostFrequentWindDirection")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("WeatherStationWindParameters");
                });

            modelBuilder.Entity("SmartHome.Domain.Entities.Devices.Shared.DeviceStatus", b =>
                {
                    b.HasOne("SmartHome.Domain.Entities.Devices.Shared.Device", "Device")
                        .WithMany("DeviceStatusHistory")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
