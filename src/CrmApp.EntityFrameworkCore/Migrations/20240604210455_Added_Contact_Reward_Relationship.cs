using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmApp.Migrations
{
    /// <inheritdoc />
    public partial class Added_Contact_Reward_Relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppContacts_RewardId",
                table: "AppContacts",
                column: "RewardId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppContacts_AppRewards_RewardId",
                table: "AppContacts",
                column: "RewardId",
                principalTable: "AppRewards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppContacts_AppRewards_RewardId",
                table: "AppContacts");

            migrationBuilder.DropIndex(
                name: "IX_AppContacts_RewardId",
                table: "AppContacts");
        }
    }
}
