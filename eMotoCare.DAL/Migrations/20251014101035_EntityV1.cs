using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMotoCare.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EntityV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchInventoryExports");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BranchInventoryExports",
                columns: table => new
                {
                    BranchInventoryExportId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BranchInventoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ExportNoteId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchInventoryExports", x => x.BranchInventoryExportId);
                    table.ForeignKey(
                        name: "FK_BranchInventoryExports_BranchInventories_BranchInventoryId",
                        column: x => x.BranchInventoryId,
                        principalTable: "BranchInventories",
                        principalColumn: "BranchInventoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchInventoryExports_ExportNotes_ExportNoteId",
                        column: x => x.ExportNoteId,
                        principalTable: "ExportNotes",
                        principalColumn: "ExportNoteId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BranchInventoryExports_BranchInventoryId",
                table: "BranchInventoryExports",
                column: "BranchInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchInventoryExports_ExportNoteId",
                table: "BranchInventoryExports",
                column: "ExportNoteId");
        }
    }
}
