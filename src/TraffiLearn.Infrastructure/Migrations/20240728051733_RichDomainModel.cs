using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraffiLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RichDomainModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answers",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "TitleDetails_TicketNumber",
                table: "Questions",
                newName: "TicketNumber");

            migrationBuilder.RenameColumn(
                name: "TitleDetails_QuestionNumber",
                table: "Questions",
                newName: "QuestionNumber");

            migrationBuilder.AlterColumn<int>(
                name: "LikesCount",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUri",
                table: "Questions",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Explanation",
                table: "Questions",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "DislikesCount",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TicketNumber",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QuestionNumber",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => new { x.QuestionId, x.Id });
                    table.ForeignKey(
                        name: "FK_Answer_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.RenameColumn(
                name: "TicketNumber",
                table: "Questions",
                newName: "TitleDetails_TicketNumber");

            migrationBuilder.RenameColumn(
                name: "QuestionNumber",
                table: "Questions",
                newName: "TitleDetails_QuestionNumber");

            migrationBuilder.AlterColumn<int>(
                name: "LikesCount",
                table: "Questions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUri",
                table: "Questions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Explanation",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<int>(
                name: "DislikesCount",
                table: "Questions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "TitleDetails_TicketNumber",
                table: "Questions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TitleDetails_QuestionNumber",
                table: "Questions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Answers",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
