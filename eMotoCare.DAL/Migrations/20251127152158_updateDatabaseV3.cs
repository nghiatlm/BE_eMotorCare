using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMotoCare.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabaseV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_export_note_detail_part_item_part_item_id",
                table: "export_note_detail");

            migrationBuilder.DropColumn(
                name: "status",
                table: "model_part");

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "part_type",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "part_item_id",
                table: "export_note_detail",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "export_index",
                table: "export_note_detail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "export_note_detail",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "total_exports",
                table: "export_note",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_export_note_detail_part_item_part_item_id",
                table: "export_note_detail",
                column: "part_item_id",
                principalTable: "part_item",
                principalColumn: "part_item_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_export_note_detail_part_item_part_item_id",
                table: "export_note_detail");

            migrationBuilder.DropColumn(
                name: "type",
                table: "part_type");

            migrationBuilder.DropColumn(
                name: "export_index",
                table: "export_note_detail");

            migrationBuilder.DropColumn(
                name: "status",
                table: "export_note_detail");

            migrationBuilder.DropColumn(
                name: "total_exports",
                table: "export_note");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "model_part",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "part_item_id",
                table: "export_note_detail",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_export_note_detail_part_item_part_item_id",
                table: "export_note_detail",
                column: "part_item_id",
                principalTable: "part_item",
                principalColumn: "part_item_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
