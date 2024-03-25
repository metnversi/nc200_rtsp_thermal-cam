using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace a.Migrations
{
    /// <inheritdoc />
    public partial class Ij : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IpAddress = table.Column<string>(type: "TEXT", nullable: true),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    IsSelected = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Temps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IpAddress = table.Column<string>(type: "TEXT", nullable: true),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MinTemp = table.Column<string>(type: "TEXT", nullable: true),
                    MaxTemp = table.Column<string>(type: "TEXT", nullable: true),
                    CamId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Temps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Temps_Cams_CamId",
                        column: x => x.CamId,
                        principalTable: "Cams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Temps_CamId",
                table: "Temps",
                column: "CamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Temps");

            migrationBuilder.DropTable(
                name: "Cams");
        }
    }
}
