using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace a.Migrations
{
    /// <inheritdoc />
    public partial class Ijgg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Temps_Cams_CamId",
                table: "Temps");

            migrationBuilder.DropIndex(
                name: "IX_Temps_CamId",
                table: "Temps");

            migrationBuilder.DropColumn(
                name: "CamId",
                table: "Temps");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CamId",
                table: "Temps",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Temps_CamId",
                table: "Temps",
                column: "CamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Temps_Cams_CamId",
                table: "Temps",
                column: "CamId",
                principalTable: "Cams",
                principalColumn: "Id");
        }
    }
}
