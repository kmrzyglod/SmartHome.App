using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartHome.Infrastructure.Migrations
{
    public partial class RenameTablesAndAddMissingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "WeatherStationAir", newName: "WeatherStationAirParameters");
            migrationBuilder.DropPrimaryKey(name: "PK_WeatherStationAir", table: "WeatherStationAirParameters");
            migrationBuilder.AddPrimaryKey(name: "PK_WeatherStationAirParameters", table: "WeatherStationAirParameters", "Id");
            
            migrationBuilder.RenameTable(
                name: "WeatherStationSun", newName: "WeatherStationInsolationParameters");
            migrationBuilder.DropPrimaryKey(name: "PK_WeatherStationSun", table: "WeatherStationInsolationParameters");
            migrationBuilder.AddPrimaryKey(name: "PK_WeatherStationInsolationParameters", table: "WeatherStationInsolationParameters", "Id");

            migrationBuilder.RenameTable(
                name: "WeatherStationWind", newName: "WeatherStationWindParameters");
            migrationBuilder.DropPrimaryKey(name: "PK_WeatherStationWind", table: "WeatherStationWindParameters");
            migrationBuilder.AddPrimaryKey(name: "PK_WeatherStationWindParameters", table: "WeatherStationWindParameters", "Id");

            migrationBuilder.CreateTable(
                name: "GreenhouseAirParameters",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Temperature = table.Column<double>(nullable: false),
                    Humidity = table.Column<double>(nullable: false),
                    Pressure = table.Column<double>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GreenhouseAirParameters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GreenhouseInsolationParameters",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LightLevelInLux = table.Column<double>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GreenhouseInsolationParameters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GreenhouseIrrigationHistory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalWaterVolume = table.Column<float>(nullable: false),
                    AverageWaterFlow = table.Column<float>(nullable: false),
                    MinWaterFlow = table.Column<float>(nullable: false),
                    MaxWaterFlow = table.Column<float>(nullable: false),
                    MeasurementStartTime = table.Column<DateTime>(nullable: false),
                    MeasurementEndTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GreenhouseIrrigationHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GreenhouseSoilParameters",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoilMoisture = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GreenhouseSoilParameters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GreenhouseWindowsStatus",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Window1Opened = table.Column<bool>(nullable: false),
                    Window2Opened = table.Column<bool>(nullable: false),
                    DoorOpened = table.Column<bool>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GreenhouseWindowsStatus", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GreenhouseAirParameters");

            migrationBuilder.DropTable(
                name: "GreenhouseInsolationParameters");

            migrationBuilder.DropTable(
                name: "GreenhouseIrrigationHistory");

            migrationBuilder.DropTable(
                name: "GreenhouseSoilParameters");

            migrationBuilder.DropTable(
                name: "GreenhouseWindowsStatus");

           
            migrationBuilder.RenameTable(
                newName: "WeatherStationAir", name: "WeatherStationAirParameters");
            migrationBuilder.DropPrimaryKey(name: "PK_WeatherStationAirParameters", table: "WeatherStationAir");
            migrationBuilder.AddPrimaryKey(name: "PK_WeatherStationAir", table: "WeatherStationAir", "Id");
            
            migrationBuilder.RenameTable(
                newName: "WeatherStationSun", name: "WeatherStationInsolationParameters");
            migrationBuilder.DropPrimaryKey(name: "PK_WeatherStationInsolationParameters", table: "WeatherStationSun");
            migrationBuilder.AddPrimaryKey(name: "PK_WeatherStationSun", table: "WeatherStationSun", "Id");

            migrationBuilder.RenameTable(
                newName: "WeatherStationWind", name: "WeatherStationWindParameters");
            migrationBuilder.DropPrimaryKey(name: "PK_WeatherStationWindParameters", table: "WeatherStationWind");
            migrationBuilder.AddPrimaryKey(name: "PK_WeatherStationWind", table: "WeatherStationWind", "Id");

        }
    }
}
