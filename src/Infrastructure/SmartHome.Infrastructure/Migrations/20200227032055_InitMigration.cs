using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartHome.Infrastructure.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherStationAir",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Temperature = table.Column<double>(nullable: false),
                    Humidity = table.Column<double>(nullable: false),
                    Pressure = table.Column<double>(nullable: false),
                    MeasurementStartTime = table.Column<System.DateTime>(nullable: false),
                    MeasurementEndTime = table.Column<System.DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherStationAir", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherStationPrecipitation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementStartTime = table.Column<DateTime>(nullable: false),
                    MeasurementEndTime = table.Column<DateTime>(nullable: false),
                    Rain = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherStationPrecipitation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherStationSun",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementStartTime = table.Column<DateTime>(nullable: false),
                    MeasurementEndTime = table.Column<DateTime>(nullable: false),
                    LightLevelInLux = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherStationSun", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherStationWind",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MostFrequentWindDirection = table.Column<int>(nullable: false),
                    CurrentWindDirection = table.Column<int>(nullable: false),
                    MinWindSpeed = table.Column<double>(nullable: false),
                    MaxWindSpeed = table.Column<double>(nullable: false),
                    AverageWindSpeed = table.Column<double>(nullable: false),
                    MeasurementStartTime = table.Column<DateTime>(nullable: false),
                    MeasurementEndTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherStationWind", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherStationAir");

            migrationBuilder.DropTable(
                name: "WeatherStationPrecipitation");

            migrationBuilder.DropTable(
                name: "WeatherStationSun");

            migrationBuilder.DropTable(
                name: "WeatherStationWind");
        }
    }
}
