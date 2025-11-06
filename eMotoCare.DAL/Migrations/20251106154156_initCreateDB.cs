using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMotoCare.DAL.Migrations
{
    /// <inheritdoc />
    public partial class initCreateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointment_service_center_slot_service_center_slot_id",
                table: "appointment"
            );

            migrationBuilder.DropIndex(
                name: "IX_appointment_service_center_slot_id",
                table: "appointment"
            );

            migrationBuilder.DropColumn(name: "end_time", table: "service_center_slot");

            migrationBuilder.DropColumn(name: "start_time", table: "service_center_slot");

            migrationBuilder.DropColumn(name: "service_center_slot_id", table: "appointment");

            migrationBuilder.AddColumn<int>(
                name: "slot_time",
                table: "service_center_slot",
                type: "int",
                nullable: false,
                defaultValue: 1
            );

            migrationBuilder.AddColumn<int>(
                name: "slot_time",
                table: "appointment",
                type: "int",
                nullable: false,
                defaultValue: 1
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "slot_time", table: "service_center_slot");

            migrationBuilder.DropColumn(name: "slot_time", table: "appointment");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "end_time",
                table: "service_center_slot",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0)
            );

            migrationBuilder.AddColumn<TimeSpan>(
                name: "start_time",
                table: "service_center_slot",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0)
            );

            migrationBuilder.AddColumn<Guid>(
                name: "service_center_slot_id",
                table: "appointment",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci"
            );

            migrationBuilder.CreateIndex(
                name: "IX_appointment_service_center_slot_id",
                table: "appointment",
                column: "service_center_slot_id"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_appointment_service_center_slot_service_center_slot_id",
                table: "appointment",
                column: "service_center_slot_id",
                principalTable: "service_center_slot",
                principalColumn: "service_center_slot_id"
            );
        }
    }
}
