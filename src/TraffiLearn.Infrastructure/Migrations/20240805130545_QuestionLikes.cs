using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraffiLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuestionLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionUser_Questions_MarkedQuestionsId",
                table: "QuestionUser");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionUser_Users_UserId",
                table: "QuestionUser");

            migrationBuilder.DropColumn(
                name: "DislikesCount",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "QuestionUser",
                newName: "LikedQuestionsId");

            migrationBuilder.RenameColumn(
                name: "MarkedQuestionsId",
                table: "QuestionUser",
                newName: "LikedByUsersId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionUser_UserId",
                table: "QuestionUser",
                newName: "IX_QuestionUser_LikedQuestionsId");

            migrationBuilder.CreateTable(
                name: "QuestionUser1",
                columns: table => new
                {
                    DislikedByUsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DislikedQuestionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionUser1", x => new { x.DislikedByUsersId, x.DislikedQuestionsId });
                    table.ForeignKey(
                        name: "FK_QuestionUser1_Questions_DislikedQuestionsId",
                        column: x => x.DislikedQuestionsId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionUser1_Users_DislikedByUsersId",
                        column: x => x.DislikedByUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionUser2",
                columns: table => new
                {
                    MarkedQuestionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionUser2", x => new { x.MarkedQuestionsId, x.UserId });
                    table.ForeignKey(
                        name: "FK_QuestionUser2_Questions_MarkedQuestionsId",
                        column: x => x.MarkedQuestionsId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionUser2_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionUser1_DislikedQuestionsId",
                table: "QuestionUser1",
                column: "DislikedQuestionsId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionUser2_UserId",
                table: "QuestionUser2",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionUser_Questions_LikedQuestionsId",
                table: "QuestionUser",
                column: "LikedQuestionsId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionUser_Users_LikedByUsersId",
                table: "QuestionUser",
                column: "LikedByUsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionUser_Questions_LikedQuestionsId",
                table: "QuestionUser");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionUser_Users_LikedByUsersId",
                table: "QuestionUser");

            migrationBuilder.DropTable(
                name: "QuestionUser1");

            migrationBuilder.DropTable(
                name: "QuestionUser2");

            migrationBuilder.RenameColumn(
                name: "LikedQuestionsId",
                table: "QuestionUser",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "LikedByUsersId",
                table: "QuestionUser",
                newName: "MarkedQuestionsId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionUser_LikedQuestionsId",
                table: "QuestionUser",
                newName: "IX_QuestionUser_UserId");

            migrationBuilder.AddColumn<int>(
                name: "DislikesCount",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionUser_Questions_MarkedQuestionsId",
                table: "QuestionUser",
                column: "MarkedQuestionsId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionUser_Users_UserId",
                table: "QuestionUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
