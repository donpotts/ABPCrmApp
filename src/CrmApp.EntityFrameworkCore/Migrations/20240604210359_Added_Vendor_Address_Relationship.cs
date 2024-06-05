using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmApp.Migrations
{
    /// <inheritdoc />
    public partial class Added_Vendor_Address_Relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppVendors_AddressId",
                table: "AppVendors",
                column: "AddressId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppVendors_AppAddresses_AddressId",
                table: "AppVendors",
                column: "AddressId",
                principalTable: "AppAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppVendors_AppAddresses_AddressId",
                table: "AppVendors");

            migrationBuilder.DropIndex(
                name: "IX_AppVendors_AddressId",
                table: "AppVendors");
        }
    }
}
