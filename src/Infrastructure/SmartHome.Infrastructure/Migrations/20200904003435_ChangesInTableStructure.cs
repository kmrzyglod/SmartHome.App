using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartHome.Infrastructure.Migrations
{
    public partial class ChangesInTableStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GreenhouseWindowsStatus");

            migrationBuilder.AlterColumn<double>(
                name: "TotalWaterVolume",
                table: "GreenhouseIrrigationHistory",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<double>(
                name: "MinWaterFlow",
                table: "GreenhouseIrrigationHistory",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<double>(
                name: "MaxWaterFlow",
                table: "GreenhouseIrrigationHistory",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<double>(
                name: "AverageWaterFlow",
                table: "GreenhouseIrrigationHistory",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "TotalWaterVolume",
                table: "GreenhouseIrrigationHistory",
                type: "real",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "MinWaterFlow",
                table: "GreenhouseIrrigationHistory",
                type: "real",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "MaxWaterFlow",
                table: "GreenhouseIrrigationHistory",
                type: "real",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "AverageWaterFlow",
                table: "GreenhouseIrrigationHistory",
                type: "real",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.CreateTable(
                name: "GreenhouseWindowsStatus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoorOpened = table.Column<bool>(type: "bit", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Window1Opened = table.Column<bool>(type: "bit", nullable: false),
                    Window2Opened = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GreenhouseWindowsStatus", x => x.Id);
                });
        }
    }
}
