using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HimariServer.Repository.Migrations
{
    /// <inheritdoc />
    public partial class FixFormatDistrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "User",
                newName: "Ward");

            migrationBuilder.AddColumn<string>(
                name: "AddressBonus",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressBonus",
                table: "User");

            migrationBuilder.DropColumn(
                name: "District",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "Ward",
                table: "User",
                newName: "Address");
        }
    }
}
