using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMotoCare.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabaseV6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date_of_implementation",
                table: "vehicle_stage",
                newName: "updated_at");

            migrationBuilder.AddColumn<DateTime>(
                name: "actual_implementation_date",
                table: "vehicle_stage",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "vehicle_stage",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "expected_end_date",
                table: "vehicle_stage",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "expected_implementation_date",
                table: "vehicle_stage",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "expected_start_date",
                table: "vehicle_stage",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "actual_implementation_date",
                table: "vehicle_stage");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "vehicle_stage");

            migrationBuilder.DropColumn(
                name: "expected_end_date",
                table: "vehicle_stage");

            migrationBuilder.DropColumn(
                name: "expected_implementation_date",
                table: "vehicle_stage");

            migrationBuilder.DropColumn(
                name: "expected_start_date",
                table: "vehicle_stage");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "vehicle_stage",
                newName: "date_of_implementation");
        }
    }
}
