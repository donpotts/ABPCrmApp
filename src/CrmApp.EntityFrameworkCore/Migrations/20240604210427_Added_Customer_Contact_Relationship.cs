using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmApp.Migrations
{
    /// <inheritdoc />
    public partial class Added_Customer_Contact_Relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppCustomers_ContactId",
                table: "AppCustomers",
                column: "ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCustomers_AppContacts_ContactId",
                table: "AppCustomers",
                column: "ContactId",
                principalTable: "AppContacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCustomers_AppContacts_ContactId",
                table: "AppCustomers");

            migrationBuilder.DropIndex(
                name: "IX_AppCustomers_ContactId",
                table: "AppCustomers");
        }
    }
}
