using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraffiLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixPreviousMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_RootCommentId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_RootCommentId",
                table: "Comments",
                column: "RootCommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_RootCommentId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_RootCommentId",
                table: "Comments",
                column: "RootCommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }
    }
}
