using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HimariServer.Repository.Migrations
{
    /// <inheritdoc />
    public partial class BlogCategoryNameUnsign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameUnsign",
                table: "BlogCategory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameUnsign",
                table: "BlogCategory");
        }
    }
}
