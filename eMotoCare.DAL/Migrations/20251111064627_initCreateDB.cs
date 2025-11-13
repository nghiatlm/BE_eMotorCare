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
            // Drop the foreign key constraint first
            migrationBuilder.DropForeignKey(
                name: "FK_service_center_inventory_service_center_service_center_id",
                table: "service_center_inventory"
            );

            // Now drop the index
            migrationBuilder.DropIndex(
                name: "IX_service_center_inventory_service_center_id",
                table: "service_center_inventory"
            );

            // Add new columns and indexes as needed
            migrationBuilder.AddColumn<Guid>(
                name: "customer_id",
                table: "rma",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci"
            );

            migrationBuilder.AddColumn<decimal>(
                name: "VAT",
                table: "ev_check",
                type: "decimal(65,30)",
                nullable: true
            );

            migrationBuilder.AddColumn<decimal>(
                name: "part_price",
                table: "ev_check",
                type: "decimal(65,30)",
                nullable: true
            );

            migrationBuilder.AddColumn<decimal>(
                name: "service_price",
                table: "ev_check",
                type: "decimal(65,30)",
                nullable: true
            );

            migrationBuilder
                .AddColumn<string>(
                    name: "note",
                    table: "appointment",
                    type: "varchar(500)",
                    nullable: true
                )
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_service_center_inventory_service_center_id",
                table: "service_center_inventory",
                column: "service_center_id",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_rma_customer_id",
                table: "rma",
                column: "customer_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_customer_citizen_id",
                table: "customer",
                column: "citizen_id",
                unique: true
            );

            migrationBuilder.AddForeignKey(
                name: "FK_rma_customer_customer_id",
                table: "rma",
                column: "customer_id",
                principalTable: "customer",
                principalColumn: "customer_id",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the foreign key constraint
            migrationBuilder.DropForeignKey(name: "FK_rma_customer_customer_id", table: "rma");

            // Drop the indexes
            migrationBuilder.DropIndex(
                name: "IX_service_center_inventory_service_center_id",
                table: "service_center_inventory"
            );

            migrationBuilder.DropIndex(name: "IX_rma_customer_id", table: "rma");

            migrationBuilder.DropIndex(name: "IX_customer_citizen_id", table: "customer");

            // Drop the columns
            migrationBuilder.DropColumn(name: "customer_id", table: "rma");

            migrationBuilder.DropColumn(name: "VAT", table: "ev_check");

            migrationBuilder.DropColumn(name: "part_price", table: "ev_check");

            migrationBuilder.DropColumn(name: "service_price", table: "ev_check");

            migrationBuilder.DropColumn(name: "note", table: "appointment");

            // Recreate the index
            migrationBuilder.CreateIndex(
                name: "IX_service_center_inventory_service_center_id",
                table: "service_center_inventory",
                column: "service_center_id"
            );

            // Recreate the foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_service_center_inventory_service_center_service_center_id",
                table: "service_center_inventory",
                column: "service_center_id",
                principalTable: "service_center",
                principalColumn: "service_center_id",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}
