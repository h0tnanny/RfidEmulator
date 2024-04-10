using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RfidEmulator.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddHasOneReaderToReaderConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Readers_ReaderConfigs_ConfigId",
                schema: "equipment",
                table: "Readers");

            migrationBuilder.DropIndex(
                name: "IX_Readers_ConfigId",
                schema: "equipment",
                table: "Readers");

            migrationBuilder.RenameTable(
                name: "ReaderConfigs",
                newName: "ReaderConfigs",
                newSchema: "equipment");

            migrationBuilder.RenameColumn(
                name: "ConfigId",
                schema: "equipment",
                table: "Readers",
                newName: "ReaderConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_ReaderConfigId",
                schema: "equipment",
                table: "Readers",
                column: "ReaderConfigId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReaderConfigs_Id",
                schema: "equipment",
                table: "ReaderConfigs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Readers_ReaderConfigs_ReaderConfigId",
                schema: "equipment",
                table: "Readers",
                column: "ReaderConfigId",
                principalSchema: "equipment",
                principalTable: "ReaderConfigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Readers_ReaderConfigs_ReaderConfigId",
                schema: "equipment",
                table: "Readers");

            migrationBuilder.DropIndex(
                name: "IX_Readers_ReaderConfigId",
                schema: "equipment",
                table: "Readers");

            migrationBuilder.DropIndex(
                name: "IX_ReaderConfigs_Id",
                schema: "equipment",
                table: "ReaderConfigs");

            migrationBuilder.RenameTable(
                name: "ReaderConfigs",
                schema: "equipment",
                newName: "ReaderConfigs");

            migrationBuilder.RenameColumn(
                name: "ReaderConfigId",
                schema: "equipment",
                table: "Readers",
                newName: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_ConfigId",
                schema: "equipment",
                table: "Readers",
                column: "ConfigId");

            migrationBuilder.AddForeignKey(
                name: "FK_Readers_ReaderConfigs_ConfigId",
                schema: "equipment",
                table: "Readers",
                column: "ConfigId",
                principalTable: "ReaderConfigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
