using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraffiLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToImageUri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "ImageUri",
                table: "Questions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUri",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Questions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
