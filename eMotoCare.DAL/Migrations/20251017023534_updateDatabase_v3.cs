using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMotoCare.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabase_v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_export_not_service_center_service_center_id",
                table: "export_not");

            migrationBuilder.DropForeignKey(
                name: "FK_export_not_staff_export_by_id",
                table: "export_not");

            migrationBuilder.DropForeignKey(
                name: "FK_part_item_export_not_export_note_id",
                table: "part_item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_export_not",
                table: "export_not");

            migrationBuilder.DropColumn(
                name: "export_id",
                table: "part_item");

            migrationBuilder.RenameTable(
                name: "export_not",
                newName: "export_note");

            migrationBuilder.RenameIndex(
                name: "IX_export_not_service_center_id",
                table: "export_note",
                newName: "IX_export_note_service_center_id");

            migrationBuilder.RenameIndex(
                name: "IX_export_not_export_by_id",
                table: "export_note",
                newName: "IX_export_note_export_by_id");

            migrationBuilder.AlterColumn<string>(
                name: "unit",
                table: "maintenance_plan",
                type: "varchar(200)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "account",
                type: "varchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_export_note",
                table: "export_note",
                column: "export_note_id");

            migrationBuilder.AddForeignKey(
                name: "FK_export_note_service_center_service_center_id",
                table: "export_note",
                column: "service_center_id",
                principalTable: "service_center",
                principalColumn: "service_center_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_export_note_staff_export_by_id",
                table: "export_note",
                column: "export_by_id",
                principalTable: "staff",
                principalColumn: "staff_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_part_item_export_note_export_note_id",
                table: "part_item",
                column: "export_note_id",
                principalTable: "export_note",
                principalColumn: "export_note_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_export_note_service_center_service_center_id",
                table: "export_note");

            migrationBuilder.DropForeignKey(
                name: "FK_export_note_staff_export_by_id",
                table: "export_note");

            migrationBuilder.DropForeignKey(
                name: "FK_part_item_export_note_export_note_id",
                table: "part_item");

            migrationBuilder.DropPrimaryKey(
                name: "PK_export_note",
                table: "export_note");

            migrationBuilder.RenameTable(
                name: "export_note",
                newName: "export_not");

            migrationBuilder.RenameIndex(
                name: "IX_export_note_service_center_id",
                table: "export_not",
                newName: "IX_export_not_service_center_id");

            migrationBuilder.RenameIndex(
                name: "IX_export_note_export_by_id",
                table: "export_not",
                newName: "IX_export_not_export_by_id");

            migrationBuilder.AddColumn<Guid>(
                name: "export_id",
                table: "part_item",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AlterColumn<int>(
                name: "unit",
                table: "maintenance_plan",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "account",
                keyColumn: "email",
                keyValue: null,
                column: "email",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "account",
                type: "varchar(200)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_export_not",
                table: "export_not",
                column: "export_note_id");

            migrationBuilder.AddForeignKey(
                name: "FK_export_not_service_center_service_center_id",
                table: "export_not",
                column: "service_center_id",
                principalTable: "service_center",
                principalColumn: "service_center_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_export_not_staff_export_by_id",
                table: "export_not",
                column: "export_by_id",
                principalTable: "staff",
                principalColumn: "staff_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_part_item_export_not_export_note_id",
                table: "part_item",
                column: "export_note_id",
                principalTable: "export_not",
                principalColumn: "export_note_id");
        }
    }
}
