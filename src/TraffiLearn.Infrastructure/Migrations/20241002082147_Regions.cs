using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraffiLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Regions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                table: "Users",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_Id",
                table: "Topics",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Id",
                table: "Tickets",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_Id",
                table: "Questions",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_Id",
                table: "Comments",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Id",
                table: "Regions",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Users_Id",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Topics_Id",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_Id",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Questions_Id",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Comments_Id",
                table: "Comments");
        }
    }
}
