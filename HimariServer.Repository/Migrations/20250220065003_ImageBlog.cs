using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HimariServer.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ImageBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Blog",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Blog");
        }
    }
}
