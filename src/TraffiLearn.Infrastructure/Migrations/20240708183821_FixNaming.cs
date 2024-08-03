using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraffiLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberDetails_TickerNumber",
                table: "Questions",
                newName: "NumberDetails_TicketNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberDetails_TicketNumber",
                table: "Questions",
                newName: "NumberDetails_TickerNumber");
        }
    }
}
