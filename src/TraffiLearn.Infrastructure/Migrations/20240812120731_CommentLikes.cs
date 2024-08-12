using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraffiLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CommentLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Comments",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                newName: "IX_Comments_CreatorId");

            migrationBuilder.CreateTable(
                name: "CommentsDislikedByUsers",
                columns: table => new
                {
                    DislikedByUsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DislikedCommentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentsDislikedByUsers", x => new { x.DislikedByUsersId, x.DislikedCommentsId });
                    table.ForeignKey(
                        name: "FK_CommentsDislikedByUsers_Comments_DislikedCommentsId",
                        column: x => x.DislikedCommentsId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentsDislikedByUsers_Users_DislikedByUsersId",
                        column: x => x.DislikedByUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentsLikedByUsers",
                columns: table => new
                {
                    LikedByUsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LikedCommentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentsLikedByUsers", x => new { x.LikedByUsersId, x.LikedCommentsId });
                    table.ForeignKey(
                        name: "FK_CommentsLikedByUsers_Comments_LikedCommentsId",
                        column: x => x.LikedCommentsId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentsLikedByUsers_Users_LikedByUsersId",
                        column: x => x.LikedByUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentsDislikedByUsers_DislikedCommentsId",
                table: "CommentsDislikedByUsers",
                column: "DislikedCommentsId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsLikedByUsers_LikedCommentsId",
                table: "CommentsLikedByUsers",
                column: "LikedCommentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_CreatorId",
                table: "Comments",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_CreatorId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "CommentsDislikedByUsers");

            migrationBuilder.DropTable(
                name: "CommentsLikedByUsers");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Comments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CreatorId",
                table: "Comments",
                newName: "IX_Comments_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
