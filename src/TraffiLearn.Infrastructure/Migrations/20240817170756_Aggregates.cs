using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraffiLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Aggregates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionsMarked");

            migrationBuilder.CreateTable(
                name: "QuestionsMarkedByUsers",
                columns: table => new
                {
                    MarkedByUsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MarkedQuestionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsMarkedByUsers", x => new { x.MarkedByUsersId, x.MarkedQuestionsId });
                    table.ForeignKey(
                        name: "FK_QuestionsMarkedByUsers_Questions_MarkedQuestionsId",
                        column: x => x.MarkedQuestionsId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionsMarkedByUsers_Users_MarkedByUsersId",
                        column: x => x.MarkedByUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsMarkedByUsers_MarkedQuestionsId",
                table: "QuestionsMarkedByUsers",
                column: "MarkedQuestionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionsMarkedByUsers");

            migrationBuilder.CreateTable(
                name: "QuestionsMarked",
                columns: table => new
                {
                    MarkedQuestionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsMarked", x => new { x.MarkedQuestionsId, x.UserId });
                    table.ForeignKey(
                        name: "FK_QuestionsMarked_Questions_MarkedQuestionsId",
                        column: x => x.MarkedQuestionsId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionsMarked_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsMarked_UserId",
                table: "QuestionsMarked",
                column: "UserId");
        }
    }
}
