using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RfidEmulator.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddNameAndDescriptionReader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "equipment",
                table: "Readers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "equipment",
                table: "Readers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "equipment",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "equipment",
                table: "Readers");
        }
    }
}
