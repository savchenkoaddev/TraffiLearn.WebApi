using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraffiLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SpecificTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionUser_Questions_LikedQuestionsId",
                table: "QuestionUser");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionUser_Users_LikedByUsersId",
                table: "QuestionUser");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionUser1_Questions_DislikedQuestionsId",
                table: "QuestionUser1");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionUser1_Users_DislikedByUsersId",
                table: "QuestionUser1");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionUser2_Questions_MarkedQuestionsId",
                table: "QuestionUser2");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionUser2_Users_UserId",
                table: "QuestionUser2");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionUser2",
                table: "QuestionUser2");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionUser1",
                table: "QuestionUser1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionUser",
                table: "QuestionUser");

            migrationBuilder.RenameTable(
                name: "QuestionUser2",
                newName: "QuestionsMarked");

            migrationBuilder.RenameTable(
                name: "QuestionUser1",
                newName: "QuestionsDislikedByUsers");

            migrationBuilder.RenameTable(
                name: "QuestionUser",
                newName: "QuestionsLikedByUsers");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionUser2_UserId",
                table: "QuestionsMarked",
                newName: "IX_QuestionsMarked_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionUser1_DislikedQuestionsId",
                table: "QuestionsDislikedByUsers",
                newName: "IX_QuestionsDislikedByUsers_DislikedQuestionsId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionUser_LikedQuestionsId",
                table: "QuestionsLikedByUsers",
                newName: "IX_QuestionsLikedByUsers_LikedQuestionsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionsMarked",
                table: "QuestionsMarked",
                columns: new[] { "MarkedQuestionsId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionsDislikedByUsers",
                table: "QuestionsDislikedByUsers",
                columns: new[] { "DislikedByUsersId", "DislikedQuestionsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionsLikedByUsers",
                table: "QuestionsLikedByUsers",
                columns: new[] { "LikedByUsersId", "LikedQuestionsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsDislikedByUsers_Questions_DislikedQuestionsId",
                table: "QuestionsDislikedByUsers",
                column: "DislikedQuestionsId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsDislikedByUsers_Users_DislikedByUsersId",
                table: "QuestionsDislikedByUsers",
                column: "DislikedByUsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsLikedByUsers_Questions_LikedQuestionsId",
                table: "QuestionsLikedByUsers",
                column: "LikedQuestionsId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsLikedByUsers_Users_LikedByUsersId",
                table: "QuestionsLikedByUsers",
                column: "LikedByUsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsMarked_Questions_MarkedQuestionsId",
                table: "QuestionsMarked",
                column: "MarkedQuestionsId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsMarked_Users_UserId",
                table: "QuestionsMarked",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionsDislikedByUsers_Questions_DislikedQuestionsId",
                table: "QuestionsDislikedByUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionsDislikedByUsers_Users_DislikedByUsersId",
                table: "QuestionsDislikedByUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionsLikedByUsers_Questions_LikedQuestionsId",
                table: "QuestionsLikedByUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionsLikedByUsers_Users_LikedByUsersId",
                table: "QuestionsLikedByUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionsMarked_Questions_MarkedQuestionsId",
                table: "QuestionsMarked");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionsMarked_Users_UserId",
                table: "QuestionsMarked");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionsMarked",
                table: "QuestionsMarked");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionsLikedByUsers",
                table: "QuestionsLikedByUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionsDislikedByUsers",
                table: "QuestionsDislikedByUsers");

            migrationBuilder.RenameTable(
                name: "QuestionsMarked",
                newName: "QuestionUser2");

            migrationBuilder.RenameTable(
                name: "QuestionsLikedByUsers",
                newName: "QuestionUser");

            migrationBuilder.RenameTable(
                name: "QuestionsDislikedByUsers",
                newName: "QuestionUser1");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionsMarked_UserId",
                table: "QuestionUser2",
                newName: "IX_QuestionUser2_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionsLikedByUsers_LikedQuestionsId",
                table: "QuestionUser",
                newName: "IX_QuestionUser_LikedQuestionsId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionsDislikedByUsers_DislikedQuestionsId",
                table: "QuestionUser1",
                newName: "IX_QuestionUser1_DislikedQuestionsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionUser2",
                table: "QuestionUser2",
                columns: new[] { "MarkedQuestionsId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionUser",
                table: "QuestionUser",
                columns: new[] { "LikedByUsersId", "LikedQuestionsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionUser1",
                table: "QuestionUser1",
                columns: new[] { "DislikedByUsersId", "DislikedQuestionsId" });

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

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionUser1_Questions_DislikedQuestionsId",
                table: "QuestionUser1",
                column: "DislikedQuestionsId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionUser1_Users_DislikedByUsersId",
                table: "QuestionUser1",
                column: "DislikedByUsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionUser2_Questions_MarkedQuestionsId",
                table: "QuestionUser2",
                column: "MarkedQuestionsId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionUser2_Users_UserId",
                table: "QuestionUser2",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
