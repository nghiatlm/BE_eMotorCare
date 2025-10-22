using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMotoCare.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabase_v4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "actual_maintenance_mileage",
                table: "vehicle_stage",
                type: "int",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<string>(
                name: "actual_maintenance_unit",
                table: "vehicle_stage",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "odometer",
                table: "ev_check",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "actual_maintenance_unit",
                table: "vehicle_stage");

            migrationBuilder.AlterColumn<DateTime>(
                name: "actual_maintenance_mileage",
                table: "vehicle_stage",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "odometer",
                table: "ev_check",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
