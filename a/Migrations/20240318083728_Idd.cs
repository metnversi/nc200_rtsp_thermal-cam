using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace a.Migrations
{
    /// <inheritdoc />
    public partial class Idd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cams",
                columns: table => new
                {
                    IpAddress = table.Column<string>(type: "TEXT", nullable: false),
                    CamName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cams", x => x.IpAddress);
                });

            migrationBuilder.CreateTable(
                name: "Temps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IpAddress = table.Column<string>(type: "TEXT", nullable: true),
                    TimeReading = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MinTemp = table.Column<string>(type: "TEXT", nullable: true),
                    MaxTemp = table.Column<string>(type: "TEXT", nullable: true),
                    CamIpAddress = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Temps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Temps_Cams_CamIpAddress",
                        column: x => x.CamIpAddress,
                        principalTable: "Cams",
                        principalColumn: "IpAddress");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Temps_CamIpAddress",
                table: "Temps",
                column: "CamIpAddress");
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
