using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RfidEmulator.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddCountTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Tags",
                schema: "equipment",
                table: "ReaderConfigs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                schema: "equipment",
                table: "ReaderConfigs");
        }
    }
}
