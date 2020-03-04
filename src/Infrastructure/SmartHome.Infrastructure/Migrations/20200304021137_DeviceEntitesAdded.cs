using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartHome.Infrastructure.Migrations
{
    public partial class DeviceEntitesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<string>(nullable: false),
                    DeviceName = table.Column<string>(nullable: false),
                    IsOnline = table.Column<bool>(nullable: false),
                    LastStatusUpdate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ssid = table.Column<string>(nullable: true),
                    Rssi = table.Column<double>(nullable: true),
                    Ip = table.Column<string>(nullable: true),
                    GatewayIp = table.Column<string>(nullable: true),
                    FreeHeapMemory = table.Column<long>(nullable: true),
                    DeviceId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceStatuses_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Device_DeviceId",
                table: "Device",
                column: "DeviceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceStatuses_DeviceId",
                table: "DeviceStatuses",
                column: "DeviceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceStatuses");

            migrationBuilder.DropTable(
                name: "Device");
        }
    }
}
