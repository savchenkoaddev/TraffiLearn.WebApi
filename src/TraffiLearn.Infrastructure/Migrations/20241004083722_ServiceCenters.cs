using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraffiLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ServiceCenters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceCenters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Address_LocationName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Address_RoadName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Address_BuildingNumber = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    Number = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    RegionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCenters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceCenters_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenters_Id",
                table: "ServiceCenters",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCenters_RegionId",
                table: "ServiceCenters",
                column: "RegionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceCenters");
        }
    }
}
