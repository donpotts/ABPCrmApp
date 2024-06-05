using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmApp.Migrations
{
    /// <inheritdoc />
    public partial class Added_Lead_Opportunity_Relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppLeads_OpportunityId",
                table: "AppLeads",
                column: "OpportunityId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppLeads_AppOpportunities_OpportunityId",
                table: "AppLeads",
                column: "OpportunityId",
                principalTable: "AppOpportunities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppLeads_AppOpportunities_OpportunityId",
                table: "AppLeads");

            migrationBuilder.DropIndex(
                name: "IX_AppLeads_OpportunityId",
                table: "AppLeads");
        }
    }
}
