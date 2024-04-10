using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RfidEmulator.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "equipment");

            migrationBuilder.CreateTable(
                name: "ReaderConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CountsPerSecTimeMin = table.Column<int>(type: "integer", nullable: false),
                    CountsPerSecTimeMax = table.Column<int>(type: "integer", nullable: false),
                    UpperRssiLevelMin = table.Column<int>(type: "integer", nullable: false),
                    UpperRssiLevelMax = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReaderConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Readers",
                schema: "equipment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ip = table.Column<string>(type: "text", nullable: false),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ConfigId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Readers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Readers_ReaderConfigs_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "ReaderConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Antennas",
                schema: "equipment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Power = table.Column<int>(type: "integer", nullable: true),
                    ReaderId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Antennas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Antennas_Readers_ReaderId",
                        column: x => x.ReaderId,
                        principalSchema: "equipment",
                        principalTable: "Readers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Antennas_Id",
                schema: "equipment",
                table: "Antennas",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Antennas_ReaderId",
                schema: "equipment",
                table: "Antennas",
                column: "ReaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_ConfigId",
                schema: "equipment",
                table: "Readers",
                column: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_Id",
                schema: "equipment",
                table: "Readers",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Antennas",
                schema: "equipment");

            migrationBuilder.DropTable(
                name: "Readers",
                schema: "equipment");

            migrationBuilder.DropTable(
                name: "ReaderConfigs");
        }
    }
}
