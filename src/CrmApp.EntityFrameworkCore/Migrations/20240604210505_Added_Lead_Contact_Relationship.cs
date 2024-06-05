using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmApp.Migrations
{
    /// <inheritdoc />
    public partial class Added_Lead_Contact_Relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppLeads_ContactId",
                table: "AppLeads",
                column: "ContactId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppLeads_AppContacts_ContactId",
                table: "AppLeads",
                column: "ContactId",
                principalTable: "AppContacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppLeads_AppContacts_ContactId",
                table: "AppLeads");

            migrationBuilder.DropIndex(
                name: "IX_AppLeads_ContactId",
                table: "AppLeads");
        }
    }
}
