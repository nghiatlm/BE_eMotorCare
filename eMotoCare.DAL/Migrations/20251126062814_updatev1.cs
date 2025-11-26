using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eMotoCare.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updatev1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ev_check_detail_part_replace_part_id",
                table: "ev_check_detail");

            migrationBuilder.RenameColumn(
                name: "replace_part_id",
                table: "ev_check_detail",
                newName: "proposed_replace_part_id");

            migrationBuilder.RenameIndex(
                name: "IX_ev_check_detail_replace_part_id",
                table: "ev_check_detail",
                newName: "IX_ev_check_detail_proposed_replace_part_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ev_check_detail_part_proposed_replace_part_id",
                table: "ev_check_detail",
                column: "proposed_replace_part_id",
                principalTable: "part",
                principalColumn: "part_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ev_check_detail_part_proposed_replace_part_id",
                table: "ev_check_detail");

            migrationBuilder.RenameColumn(
                name: "proposed_replace_part_id",
                table: "ev_check_detail",
                newName: "replace_part_id");

            migrationBuilder.RenameIndex(
                name: "IX_ev_check_detail_proposed_replace_part_id",
                table: "ev_check_detail",
                newName: "IX_ev_check_detail_replace_part_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ev_check_detail_part_replace_part_id",
                table: "ev_check_detail",
                column: "replace_part_id",
                principalTable: "part",
                principalColumn: "part_id");
        }
    }
}
