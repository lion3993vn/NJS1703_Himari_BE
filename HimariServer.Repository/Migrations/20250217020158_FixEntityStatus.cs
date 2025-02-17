using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HimariServer.Repository.Migrations
{
    /// <inheritdoc />
    public partial class FixEntityStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Brand");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "BodyPart");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Blog");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "User",
                type: "varchar(max)",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Product",
                type: "varchar(max)",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Order",
                type: "varchar(max)",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Category",
                type: "varchar(max)",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Brand",
                type: "varchar(max)",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "BodyPart",
                type: "varchar(max)",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Blog",
                type: "varchar(max)",
                unicode: false,
                nullable: true);
        }
    }
}
