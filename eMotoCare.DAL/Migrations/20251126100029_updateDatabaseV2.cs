using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMotoCare.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabaseV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "proposed_replace_part_id",
                table: "export_note_detail",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_export_note_detail_proposed_replace_part_id",
                table: "export_note_detail",
                column: "proposed_replace_part_id");

            migrationBuilder.AddForeignKey(
                name: "FK_export_note_detail_part_proposed_replace_part_id",
                table: "export_note_detail",
                column: "proposed_replace_part_id",
                principalTable: "part",
                principalColumn: "part_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_export_note_detail_part_proposed_replace_part_id",
                table: "export_note_detail");

            migrationBuilder.DropIndex(
                name: "IX_export_note_detail_proposed_replace_part_id",
                table: "export_note_detail");

            migrationBuilder.DropColumn(
                name: "proposed_replace_part_id",
                table: "export_note_detail");
        }
    }
}
