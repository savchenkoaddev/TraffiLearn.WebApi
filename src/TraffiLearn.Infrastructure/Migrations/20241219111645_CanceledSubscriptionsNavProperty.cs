using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraffiLearn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CanceledSubscriptionsNavProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CanceledSubscriptions_SubscriptionPlanId",
                table: "CanceledSubscriptions",
                column: "SubscriptionPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_CanceledSubscriptions_SubscriptionPlans_SubscriptionPlanId",
                table: "CanceledSubscriptions",
                column: "SubscriptionPlanId",
                principalTable: "SubscriptionPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CanceledSubscriptions_SubscriptionPlans_SubscriptionPlanId",
                table: "CanceledSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_CanceledSubscriptions_SubscriptionPlanId",
                table: "CanceledSubscriptions");
        }
    }
}
