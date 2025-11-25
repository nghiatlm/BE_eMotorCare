using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMotoCare.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabasV5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "import_note_status",
                table: "import_note");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "import_note_status",
                table: "import_note",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
