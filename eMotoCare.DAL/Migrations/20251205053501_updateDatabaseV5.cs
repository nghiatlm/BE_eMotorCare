using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMotoCare.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabaseV5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "solution",
                table: "battery_check",
                type: "nvarchar(3000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "charge_discharge_efficiency",
                table: "battery_check",
                type: "json",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "degradation_status",
                table: "battery_check",
                type: "json",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "energy_capability",
                table: "battery_check",
                type: "json",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "remaining_useful_life",
                table: "battery_check",
                type: "json",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "safety",
                table: "battery_check",
                type: "json",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "charge_discharge_efficiency",
                table: "battery_check");

            migrationBuilder.DropColumn(
                name: "degradation_status",
                table: "battery_check");

            migrationBuilder.DropColumn(
                name: "energy_capability",
                table: "battery_check");

            migrationBuilder.DropColumn(
                name: "remaining_useful_life",
                table: "battery_check");

            migrationBuilder.DropColumn(
                name: "safety",
                table: "battery_check");

            migrationBuilder.AlterColumn<string>(
                name: "solution",
                table: "battery_check",
                type: "nvarchar(400)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldNullable: true);
        }
    }
}
