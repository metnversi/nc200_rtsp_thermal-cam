using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace a.Migrations
{
    /// <inheritdoc />
    public partial class Area7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "Actives",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Actives_AreaId",
                table: "Actives",
                column: "AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actives_Areas_AreaId",
                table: "Actives",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actives_Areas_AreaId",
                table: "Actives");

            migrationBuilder.DropIndex(
                name: "IX_Actives_AreaId",
                table: "Actives");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "Actives");
        }
    }
}
