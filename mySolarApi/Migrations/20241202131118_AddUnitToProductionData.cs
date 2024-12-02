using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mySolarApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitToProductionData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionData_SolarPowerPlants_SolarPowerPlantId1",
                table: "ProductionData");

            migrationBuilder.DropIndex(
                name: "IX_ProductionData_SolarPowerPlantId1",
                table: "ProductionData");

            migrationBuilder.DropColumn(
                name: "SolarPowerPlantId1",
                table: "ProductionData");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "ProductionData",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unit",
                table: "ProductionData");

            migrationBuilder.AddColumn<int>(
                name: "SolarPowerPlantId1",
                table: "ProductionData",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionData_SolarPowerPlantId1",
                table: "ProductionData",
                column: "SolarPowerPlantId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionData_SolarPowerPlants_SolarPowerPlantId1",
                table: "ProductionData",
                column: "SolarPowerPlantId1",
                principalTable: "SolarPowerPlants",
                principalColumn: "Id");
        }
    }
}
