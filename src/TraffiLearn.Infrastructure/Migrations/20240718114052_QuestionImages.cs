using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraffiLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuestionImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Questions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Questions");
        }
    }
}
