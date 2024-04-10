using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RfidEmulator.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsEnabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                schema: "equipment",
                table: "Readers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                schema: "equipment",
                table: "Readers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
