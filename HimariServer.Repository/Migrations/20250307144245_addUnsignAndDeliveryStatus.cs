using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HimariServer.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addUnsignAndDeliveryStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductNameUnsign",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameUnsign",
                table: "PartSymptom",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryStatus",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CategoryNameUnsign",
                table: "Category",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrandNameUnsign",
                table: "Brand",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BodyPartNameUnsign",
                table: "BodyPart",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleUnsign",
                table: "Blog",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductNameUnsign",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "NameUnsign",
                table: "PartSymptom");

            migrationBuilder.DropColumn(
                name: "DeliveryStatus",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CategoryNameUnsign",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "BrandNameUnsign",
                table: "Brand");

            migrationBuilder.DropColumn(
                name: "BodyPartNameUnsign",
                table: "BodyPart");

            migrationBuilder.DropColumn(
                name: "TitleUnsign",
                table: "Blog");
        }
    }
}
