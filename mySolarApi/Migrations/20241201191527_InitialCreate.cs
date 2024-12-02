using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mySolarApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SolarPowerPlants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    InstalledPower = table.Column<decimal>(type: "TEXT", nullable: false),
                    DateOfInstallation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Latitude = table.Column<decimal>(type: "TEXT", nullable: false),
                    Longitude = table.Column<decimal>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolarPowerPlants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Production = table.Column<decimal>(type: "TEXT", nullable: false),
                    TimeseriesType = table.Column<string>(type: "TEXT", nullable: false),
                    SolarPowerPlantId = table.Column<int>(type: "INTEGER", nullable: false),
                    SolarPowerPlantName = table.Column<string>(type: "TEXT", nullable: true),
                    SolarPowerPlantId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionData_SolarPowerPlants_SolarPowerPlantId",
                        column: x => x.SolarPowerPlantId,
                        principalTable: "SolarPowerPlants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionData_SolarPowerPlants_SolarPowerPlantId1",
                        column: x => x.SolarPowerPlantId1,
                        principalTable: "SolarPowerPlants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionData_SolarPowerPlantId",
                table: "ProductionData",
                column: "SolarPowerPlantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionData_SolarPowerPlantId1",
                table: "ProductionData",
                column: "SolarPowerPlantId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionData");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "SolarPowerPlants");
        }
    }
}
