using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmApp.Migrations
{
    /// <inheritdoc />
    public partial class Added_Vendor_Service_Relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppVendors_ServiceId",
                table: "AppVendors",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppVendors_AppServices_ServiceId",
                table: "AppVendors",
                column: "ServiceId",
                principalTable: "AppServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppVendors_AppServices_ServiceId",
                table: "AppVendors");

            migrationBuilder.DropIndex(
                name: "IX_AppVendors_ServiceId",
                table: "AppVendors");
        }
    }
}
