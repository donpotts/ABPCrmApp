using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmApp.Migrations
{
    /// <inheritdoc />
    public partial class Added_Customer_Address_Relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppCustomers_AddressId",
                table: "AppCustomers",
                column: "AddressId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppCustomers_AppAddresses_AddressId",
                table: "AppCustomers",
                column: "AddressId",
                principalTable: "AppAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCustomers_AppAddresses_AddressId",
                table: "AppCustomers");

            migrationBuilder.DropIndex(
                name: "IX_AppCustomers_AddressId",
                table: "AppCustomers");
        }
    }
}
