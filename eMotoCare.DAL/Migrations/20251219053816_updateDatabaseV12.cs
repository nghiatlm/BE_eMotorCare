using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMotoCare.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabaseV12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "service_center_slot",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "service_center_slot",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "service_center_slot");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "service_center_slot");
        }
    }
}
