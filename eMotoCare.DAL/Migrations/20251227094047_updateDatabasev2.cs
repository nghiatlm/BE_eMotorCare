using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMotoCare.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabasev2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bonus_amount",
                table: "program_detail");

            migrationBuilder.DropColumn(
                name: "service_type",
                table: "program_detail");

            migrationBuilder.DropColumn(
                name: "attachment_url",
                table: "program");

            migrationBuilder.DropColumn(
                name: "title",
                table: "program");

            migrationBuilder.RenameColumn(
                name: "recall_action",
                table: "program_detail",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "discount_percent",
                table: "program_detail",
                newName: "manufacture_year");

            migrationBuilder.AddColumn<string>(
                name: "action_type",
                table: "program_detail",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "model_id",
                table: "program_detail",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "program_detail",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "created_by",
                table: "program",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "program_code",
                table: "program",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "program_type",
                table: "program",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "severity_level",
                table: "program",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_program_detail_model_id",
                table: "program_detail",
                column: "model_id");

            migrationBuilder.AddForeignKey(
                name: "FK_program_detail_model_model_id",
                table: "program_detail",
                column: "model_id",
                principalTable: "model",
                principalColumn: "model_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_program_detail_model_model_id",
                table: "program_detail");

            migrationBuilder.DropIndex(
                name: "IX_program_detail_model_id",
                table: "program_detail");

            migrationBuilder.DropColumn(
                name: "action_type",
                table: "program_detail");

            migrationBuilder.DropColumn(
                name: "model_id",
                table: "program_detail");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "program_detail");

            migrationBuilder.DropColumn(
                name: "program_code",
                table: "program");

            migrationBuilder.DropColumn(
                name: "program_type",
                table: "program");

            migrationBuilder.DropColumn(
                name: "severity_level",
                table: "program");

            migrationBuilder.RenameColumn(
                name: "manufacture_year",
                table: "program_detail",
                newName: "discount_percent");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "program_detail",
                newName: "recall_action");

            migrationBuilder.AddColumn<int>(
                name: "bonus_amount",
                table: "program_detail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "service_type",
                table: "program_detail",
                type: "varchar(100)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "created_by",
                table: "program",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "attachment_url",
                table: "program",
                type: "nvarchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "program",
                type: "nvarchar(200)",
                nullable: false,
                defaultValue: "");
        }
    }
}
